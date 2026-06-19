# Newline STV RS-232 Display Driver (Crestron)

A Crestron control module for Newline STV-series displays over an **RS-232 serial**
connection. It is built as a SIMPL# (C#) library that implements the protocol and
command sequencing, paired with a SIMPL+ wrapper that bridges the library to the
processor's serial Tx/Rx signals.

## Architecture

| File | Role |
|------|------|
| `NewlineSTV/DisplayController.cs` | SIMPL# library — builds protocol frames, manages a serialized command queue with ack/timeout handling, and parses inbound responses. Transport-agnostic. |
| `NewlineRS232.usp` | SIMPL+ wrapper — maps Crestron signals to the library's methods, and converts between raw serial bytes and the hex strings the library uses. |

### Why hex strings?

Unlike a TCP driver, this library does **not** own the serial port. SIMPL#
delegates exchange data as strings, which is awkward for raw binary (the protocol
uses bytes such as `0x00`). To stay binary-safe, the library and wrapper exchange
data as **hex strings**:

- **Outbound:** the library raises `TransmitSerialData` with a hex string; the
  wrapper converts it back to raw bytes and drives `Tx$`.
- **Inbound:** the wrapper converts incoming `Rx$` bytes to a hex string and calls
  `ReceiveHexData`, which the library re-assembles into bytes and parses.

This keeps the actual COM-port wiring in SIMPL Windows while all protocol logic
lives in the library.

## Protocol

Commands and responses are fixed-structure binary frames:

| Field | Value |
|-------|-------|
| Start byte | `0x7F` |
| Length | payload length (byte 1) |
| Header signature | `99 A2 B3 C4 02 FF` (bytes 2–7) |
| Command category / command | bytes 8–9 |
| Data | byte 10+ |
| Terminator | `0xCF` |

Total frame length is `length + 3`. The parser validates the start byte, the full
header signature, a sane payload length (4–32), and the trailing terminator before
accepting a message.

## The library — `DisplayController`

### Public methods

| Method | Description |
|--------|-------------|
| `PowerOn()` / `PowerOff()` | Power control (ack-tracked). |
| `SetInput(ushort 1–3)` | Select an input by logical index. |
| `PollPower()` / `PollInput()` / `PollVolume()` / `PollMute()` | Query current state. |
| `SetVolume(ushort)` / `SetMute(ushort)` / `ToggleMute()` | Volume / mute control (present in the library but not exposed by the current wrapper — see Scope). |

### Callbacks

`OnPowerChange`, `OnInputChange`, `OnVolumeChange`, `OnMuteChange`
(`StateChangeHandler(ushort)`), plus `TransmitSerialData(SimplSharpString)` for
outbound serial.

### Command queue

Commands are sent one at a time. Ack-tracked commands (power, input) wait for the
matching response and advance on acknowledgment or after a 2-second timeout;
volume/mute are sent fire-and-forget to avoid UI lag. Inbound parsing runs against
a buffered byte stream with a 2048-byte overflow guard.

> Concurrency note: response handling snapshots the active command under a lock so a
> command timeout firing mid-parse cannot disrupt power/input acknowledgment.

## The SIMPL+ wrapper — `NewlineRS232.usp`

### Inputs

| Signal | Action |
|--------|--------|
| `Power_On` / `Power_Off` | Power control |
| `Poll_Power` / `Poll_Input` / `Poll_Volume` | Query current state |
| `Input_Poll[1..3]` | Select an input by logical index (1, 2, or 3) |
| `Set_Volume_100` | Sets volume to 100 (see Scope) |
| `Rx$` | Raw serial data in from the COM port |

### Outputs

| Signal | Type | Meaning |
|--------|------|---------|
| `Is_Power_On_fb` | Digital | Current power state |
| `Current_Input_fb` | Analog | Currently selected input (logical index 1–3) |
| `Current_Volume_Out` | Analog | Current volume |
| `Tx$` | String | Raw serial data out to the COM port |

On startup, `Main()` registers the callbacks and polls power, input, and volume to
seed the feedback signals.

## Scope / known limitations

This module was built for a project that only required **power and input control**,
so the wrapper intentionally exposes a limited surface:

- **Volume** is exposed only as a fixed `Set_Volume_100` trigger; there is no
  variable-volume input wired up.
- **Mute** is not surfaced — the library supports it, but the wrapper's mute
  callback is a no-op and no mute input/feedback signals are defined.

The underlying library implements `SetVolume`, `SetMute`, and `ToggleMute`, so these
can be wired up later without C# changes.

## Building and deploying

1. Open `NewlineSTV/` in Visual Studio with the Crestron SimplSharp SDK and build
   the library.
2. Compile `NewlineRS232.usp` in SIMPL+ / SIMPL Windows. It links the library via
   `#USER_SIMPLSHARP_LIBRARY "NewLineSTV"`.
3. Place the module in your program, wire `Tx$`/`Rx$` to the processor's COM port
   (configured for the display's serial settings), and connect the control signals.

> Note: the SIMPL+ compiler regenerates `SPlsWork/` on every build — those are
> generated artifacts and should not be edited by hand.
