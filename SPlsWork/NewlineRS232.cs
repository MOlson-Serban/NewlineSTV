using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using Crestron;
using Crestron.Logos.SplusLibrary;
using Crestron.Logos.SplusObjects;
using Crestron.SimplSharp;
using NewLineSTV;
using Crestron.SimplSharp.SimplSharpCloudClient;
using Crestron.SimplSharp.Python;

namespace UserModule_NEWLINERS232
{
    public class UserModuleClass_NEWLINERS232 : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        
        
        Crestron.Logos.SplusObjects.DigitalInput POWER_ON;
        Crestron.Logos.SplusObjects.DigitalInput POWER_OFF;
        Crestron.Logos.SplusObjects.DigitalInput POLL_POWER;
        Crestron.Logos.SplusObjects.DigitalInput POLL_VOLUME;
        Crestron.Logos.SplusObjects.DigitalInput POLL_MUTE;
        Crestron.Logos.SplusObjects.DigitalInput POLL_INPUT;
        Crestron.Logos.SplusObjects.DigitalInput VOLUME_UP;
        Crestron.Logos.SplusObjects.DigitalInput VOLUME_DOWN;
        Crestron.Logos.SplusObjects.DigitalInput MUTE_ON;
        Crestron.Logos.SplusObjects.DigitalInput MUTE_OFF;
        Crestron.Logos.SplusObjects.DigitalInput MUTE_TOGGLE;
        InOutArray<Crestron.Logos.SplusObjects.DigitalInput> INPUT_POLL;
        Crestron.Logos.SplusObjects.StringInput RX__DOLLAR__;
        Crestron.Logos.SplusObjects.DigitalOutput IS_POWER_ON_FB;
        Crestron.Logos.SplusObjects.DigitalOutput IS_MUTED_FB;
        Crestron.Logos.SplusObjects.AnalogOutput CURRENT_VOLUME_OUT;
        Crestron.Logos.SplusObjects.AnalogOutput CURRENT_INPUT_FB;
        Crestron.Logos.SplusObjects.StringOutput TX__DOLLAR__;
        NewLineSTV.DisplayController MYDISPLAY;
        ushort ICURRENTVOL = 0;
        ushort ICURRENTMUTE = 0;
        public void ONPOWERCALLBACK ( ushort STATE ) 
            { 
            try
            {
                SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
                
                __context__.SourceCodeLine = 30;
                IS_POWER_ON_FB  .Value = (ushort) ( STATE ) ; 
                
                
            }
            finally { ObjectFinallyHandler(); }
            }
            
        public void ONINPUTCALLBACK ( ushort VAL ) 
            { 
            try
            {
                SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
                
                __context__.SourceCodeLine = 31;
                CURRENT_INPUT_FB  .Value = (ushort) ( VAL ) ; 
                
                
            }
            finally { ObjectFinallyHandler(); }
            }
            
        public void ONVOLUMECALLBACK ( ushort VAL ) 
            { 
            try
            {
                SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
                
                __context__.SourceCodeLine = 35;
                ICURRENTVOL = (ushort) ( VAL ) ; 
                __context__.SourceCodeLine = 36;
                CURRENT_VOLUME_OUT  .Value = (ushort) ( VAL ) ; 
                
                
            }
            finally { ObjectFinallyHandler(); }
            }
            
        public void ONMUTECALLBACK ( ushort STATE ) 
            { 
            try
            {
                SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
                
                __context__.SourceCodeLine = 41;
                ICURRENTMUTE = (ushort) ( STATE ) ; 
                __context__.SourceCodeLine = 42;
                IS_MUTED_FB  .Value = (ushort) ( STATE ) ; 
                
                
            }
            finally { ObjectFinallyHandler(); }
            }
            
        public void ONTRANSMITSERIAL ( SimplSharpString HEXDATA ) 
            { 
            ushort I = 0;
            
            CrestronString RAWBYTES__DOLLAR__;
            RAWBYTES__DOLLAR__  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 512, this );
            
            try
            {
                SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
                
                __context__.SourceCodeLine = 49;
                RAWBYTES__DOLLAR__  =  ( ""  )  .ToString() ; 
                __context__.SourceCodeLine = 51;
                ushort __FN_FORSTART_VAL__1 = (ushort) ( 1 ) ;
                ushort __FN_FOREND_VAL__1 = (ushort)Functions.Length( HEXDATA .ToString() .ToString() ); 
                int __FN_FORSTEP_VAL__1 = (int)2; 
                for ( I  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (I  >= __FN_FORSTART_VAL__1) && (I  <= __FN_FOREND_VAL__1) ) : ( (I  <= __FN_FORSTART_VAL__1) && (I  >= __FN_FOREND_VAL__1) ) ; I  += (ushort)__FN_FORSTEP_VAL__1) 
                    { 
                    __context__.SourceCodeLine = 53;
                    RAWBYTES__DOLLAR__  =  ( RAWBYTES__DOLLAR__ + Functions.Chr (  (int) ( Functions.HextoI( Functions.Mid( HEXDATA .ToString() , (int)( I ) , (int)( 2 ) ) ) ) )  )  .ToString() ; 
                    __context__.SourceCodeLine = 51;
                    } 
                
                __context__.SourceCodeLine = 55;
                TX__DOLLAR__  .UpdateValue ( RAWBYTES__DOLLAR__  ) ; 
                
                
            }
            finally { ObjectFinallyHandler(); }
            }
            
        object RX__DOLLAR___OnChange_0 ( Object __EventInfo__ )
        
            { 
            Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
            try
            {
                SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
                ushort I = 0;
                ushort B = 0;
                
                CrestronString HEXSTR__DOLLAR__;
                HEXSTR__DOLLAR__  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 4096, this );
                
                
                __context__.SourceCodeLine = 63;
                HEXSTR__DOLLAR__  .UpdateValue ( ""  ) ; 
                __context__.SourceCodeLine = 65;
                ushort __FN_FORSTART_VAL__1 = (ushort) ( 1 ) ;
                ushort __FN_FOREND_VAL__1 = (ushort)Functions.Length( RX__DOLLAR__ ); 
                int __FN_FORSTEP_VAL__1 = (int)1; 
                for ( I  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (I  >= __FN_FORSTART_VAL__1) && (I  <= __FN_FOREND_VAL__1) ) : ( (I  <= __FN_FORSTART_VAL__1) && (I  >= __FN_FOREND_VAL__1) ) ; I  += (ushort)__FN_FORSTEP_VAL__1) 
                    { 
                    __context__.SourceCodeLine = 67;
                    B = (ushort) ( Byte( RX__DOLLAR__ , (int)( I ) ) ) ; 
                    __context__.SourceCodeLine = 69;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( B < 16 ))  ) ) 
                        { 
                        __context__.SourceCodeLine = 71;
                        HEXSTR__DOLLAR__  .UpdateValue ( HEXSTR__DOLLAR__ + "0" + Functions.ItoHex (  (int) ( B ) )  ) ; 
                        } 
                    
                    else 
                        { 
                        __context__.SourceCodeLine = 75;
                        HEXSTR__DOLLAR__  .UpdateValue ( HEXSTR__DOLLAR__ + Functions.ItoHex (  (int) ( B ) )  ) ; 
                        } 
                    
                    __context__.SourceCodeLine = 65;
                    } 
                
                __context__.SourceCodeLine = 79;
                MYDISPLAY . ReceiveHexData ( HEXSTR__DOLLAR__ .ToSimplSharpString()) ; 
                
                
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler( __SignalEventArg__ ); }
            return this;
            
        }
        
    object POWER_ON_OnPush_1 ( Object __EventInfo__ )
    
        { 
        Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
        try
        {
            SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
            
            __context__.SourceCodeLine = 82;
            MYDISPLAY . PowerOn ( ) ; 
            
            
        }
        catch(Exception e) { ObjectCatchHandler(e); }
        finally { ObjectFinallyHandler( __SignalEventArg__ ); }
        return this;
        
    }
    
object POWER_OFF_OnPush_2 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 83;
        MYDISPLAY . PowerOff ( ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object POLL_POWER_OnPush_3 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 84;
        MYDISPLAY . PollPower ( ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object POLL_VOLUME_OnPush_4 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 85;
        MYDISPLAY . PollVolume ( ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object POLL_MUTE_OnPush_5 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 86;
        MYDISPLAY . PollMute ( ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object POLL_INPUT_OnPush_6 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 87;
        MYDISPLAY . PollInput ( ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object INPUT_POLL_OnPush_7 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort I = 0;
        
        
        __context__.SourceCodeLine = 92;
        I = (ushort) ( Functions.GetLastModifiedArrayIndex( __SignalEventArg__ ) ) ; 
        __context__.SourceCodeLine = 93;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( I > 0 ) ) && Functions.TestForTrue ( Functions.BoolToInt ( I <= 3 ) )) ))  ) ) 
            { 
            __context__.SourceCodeLine = 93;
            MYDISPLAY . SetInput ( (ushort)( I )) ; 
            } 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object VOLUME_UP_OnPush_8 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 96;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( ICURRENTVOL < 100 ))  ) ) 
            { 
            __context__.SourceCodeLine = 96;
            MYDISPLAY . SetVolume ( (ushort)( (ICURRENTVOL + 1) )) ; 
            } 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object VOLUME_DOWN_OnPush_9 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 97;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( ICURRENTVOL > 0 ))  ) ) 
            { 
            __context__.SourceCodeLine = 97;
            MYDISPLAY . SetVolume ( (ushort)( (ICURRENTVOL - 1) )) ; 
            } 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object VOLUME_UP_OnRelease_10 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 102;
        MYDISPLAY . PollVolume ( ) ; 
        __context__.SourceCodeLine = 103;
        MYDISPLAY . PollMute ( ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object MUTE_ON_OnPush_11 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 106;
        MYDISPLAY . SetMute ( (ushort)( 1 )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object MUTE_OFF_OnPush_12 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 107;
        MYDISPLAY . SetMute ( (ushort)( 0 )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object MUTE_TOGGLE_OnPush_13 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 108;
        MYDISPLAY . ToggleMute ( ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object MUTE_TOGGLE_OnRelease_14 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 112;
        MYDISPLAY . PollMute ( ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

public override object FunctionMain (  object __obj__ ) 
    { 
    try
    {
        SplusExecutionContext __context__ = SplusFunctionMainStartCode();
        
        __context__.SourceCodeLine = 118;
        WaitForInitializationComplete ( ) ; 
        __context__.SourceCodeLine = 120;
        ICURRENTVOL = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 121;
        ICURRENTMUTE = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 123;
        // RegisterDelegate( MYDISPLAY , ONPOWERCHANGE , ONPOWERCALLBACK ) 
        MYDISPLAY .OnPowerChange  = ONPOWERCALLBACK; ; 
        __context__.SourceCodeLine = 124;
        // RegisterDelegate( MYDISPLAY , ONINPUTCHANGE , ONINPUTCALLBACK ) 
        MYDISPLAY .OnInputChange  = ONINPUTCALLBACK; ; 
        __context__.SourceCodeLine = 125;
        // RegisterDelegate( MYDISPLAY , ONVOLUMECHANGE , ONVOLUMECALLBACK ) 
        MYDISPLAY .OnVolumeChange  = ONVOLUMECALLBACK; ; 
        __context__.SourceCodeLine = 126;
        // RegisterDelegate( MYDISPLAY , ONMUTECHANGE , ONMUTECALLBACK ) 
        MYDISPLAY .OnMuteChange  = ONMUTECALLBACK; ; 
        __context__.SourceCodeLine = 127;
        // RegisterDelegate( MYDISPLAY , TRANSMITSERIALDATA , ONTRANSMITSERIAL ) 
        MYDISPLAY .TransmitSerialData  = ONTRANSMITSERIAL; ; 
        __context__.SourceCodeLine = 129;
        MYDISPLAY . PollPower ( ) ; 
        __context__.SourceCodeLine = 130;
        MYDISPLAY . PollInput ( ) ; 
        __context__.SourceCodeLine = 131;
        MYDISPLAY . PollVolume ( ) ; 
        __context__.SourceCodeLine = 132;
        MYDISPLAY . PollMute ( ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler(); }
    return __obj__;
    }
    

public override void LogosSplusInitialize()
{
    SocketInfo __socketinfo__ = new SocketInfo( 1, this );
    InitialParametersClass.ResolveHostName = __socketinfo__.ResolveHostName;
    _SplusNVRAM = new SplusNVRAM( this );
    
    POWER_ON = new Crestron.Logos.SplusObjects.DigitalInput( POWER_ON__DigitalInput__, this );
    m_DigitalInputList.Add( POWER_ON__DigitalInput__, POWER_ON );
    
    POWER_OFF = new Crestron.Logos.SplusObjects.DigitalInput( POWER_OFF__DigitalInput__, this );
    m_DigitalInputList.Add( POWER_OFF__DigitalInput__, POWER_OFF );
    
    POLL_POWER = new Crestron.Logos.SplusObjects.DigitalInput( POLL_POWER__DigitalInput__, this );
    m_DigitalInputList.Add( POLL_POWER__DigitalInput__, POLL_POWER );
    
    POLL_VOLUME = new Crestron.Logos.SplusObjects.DigitalInput( POLL_VOLUME__DigitalInput__, this );
    m_DigitalInputList.Add( POLL_VOLUME__DigitalInput__, POLL_VOLUME );
    
    POLL_MUTE = new Crestron.Logos.SplusObjects.DigitalInput( POLL_MUTE__DigitalInput__, this );
    m_DigitalInputList.Add( POLL_MUTE__DigitalInput__, POLL_MUTE );
    
    POLL_INPUT = new Crestron.Logos.SplusObjects.DigitalInput( POLL_INPUT__DigitalInput__, this );
    m_DigitalInputList.Add( POLL_INPUT__DigitalInput__, POLL_INPUT );
    
    VOLUME_UP = new Crestron.Logos.SplusObjects.DigitalInput( VOLUME_UP__DigitalInput__, this );
    m_DigitalInputList.Add( VOLUME_UP__DigitalInput__, VOLUME_UP );
    
    VOLUME_DOWN = new Crestron.Logos.SplusObjects.DigitalInput( VOLUME_DOWN__DigitalInput__, this );
    m_DigitalInputList.Add( VOLUME_DOWN__DigitalInput__, VOLUME_DOWN );
    
    MUTE_ON = new Crestron.Logos.SplusObjects.DigitalInput( MUTE_ON__DigitalInput__, this );
    m_DigitalInputList.Add( MUTE_ON__DigitalInput__, MUTE_ON );
    
    MUTE_OFF = new Crestron.Logos.SplusObjects.DigitalInput( MUTE_OFF__DigitalInput__, this );
    m_DigitalInputList.Add( MUTE_OFF__DigitalInput__, MUTE_OFF );
    
    MUTE_TOGGLE = new Crestron.Logos.SplusObjects.DigitalInput( MUTE_TOGGLE__DigitalInput__, this );
    m_DigitalInputList.Add( MUTE_TOGGLE__DigitalInput__, MUTE_TOGGLE );
    
    INPUT_POLL = new InOutArray<DigitalInput>( 3, this );
    for( uint i = 0; i < 3; i++ )
    {
        INPUT_POLL[i+1] = new Crestron.Logos.SplusObjects.DigitalInput( INPUT_POLL__DigitalInput__ + i, INPUT_POLL__DigitalInput__, this );
        m_DigitalInputList.Add( INPUT_POLL__DigitalInput__ + i, INPUT_POLL[i+1] );
    }
    
    IS_POWER_ON_FB = new Crestron.Logos.SplusObjects.DigitalOutput( IS_POWER_ON_FB__DigitalOutput__, this );
    m_DigitalOutputList.Add( IS_POWER_ON_FB__DigitalOutput__, IS_POWER_ON_FB );
    
    IS_MUTED_FB = new Crestron.Logos.SplusObjects.DigitalOutput( IS_MUTED_FB__DigitalOutput__, this );
    m_DigitalOutputList.Add( IS_MUTED_FB__DigitalOutput__, IS_MUTED_FB );
    
    CURRENT_VOLUME_OUT = new Crestron.Logos.SplusObjects.AnalogOutput( CURRENT_VOLUME_OUT__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( CURRENT_VOLUME_OUT__AnalogSerialOutput__, CURRENT_VOLUME_OUT );
    
    CURRENT_INPUT_FB = new Crestron.Logos.SplusObjects.AnalogOutput( CURRENT_INPUT_FB__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( CURRENT_INPUT_FB__AnalogSerialOutput__, CURRENT_INPUT_FB );
    
    RX__DOLLAR__ = new Crestron.Logos.SplusObjects.StringInput( RX__DOLLAR____AnalogSerialInput__, 1024, this );
    m_StringInputList.Add( RX__DOLLAR____AnalogSerialInput__, RX__DOLLAR__ );
    
    TX__DOLLAR__ = new Crestron.Logos.SplusObjects.StringOutput( TX__DOLLAR____AnalogSerialOutput__, this );
    m_StringOutputList.Add( TX__DOLLAR____AnalogSerialOutput__, TX__DOLLAR__ );
    
    
    RX__DOLLAR__.OnSerialChange.Add( new InputChangeHandlerWrapper( RX__DOLLAR___OnChange_0, false ) );
    POWER_ON.OnDigitalPush.Add( new InputChangeHandlerWrapper( POWER_ON_OnPush_1, false ) );
    POWER_OFF.OnDigitalPush.Add( new InputChangeHandlerWrapper( POWER_OFF_OnPush_2, false ) );
    POLL_POWER.OnDigitalPush.Add( new InputChangeHandlerWrapper( POLL_POWER_OnPush_3, false ) );
    POLL_VOLUME.OnDigitalPush.Add( new InputChangeHandlerWrapper( POLL_VOLUME_OnPush_4, false ) );
    POLL_MUTE.OnDigitalPush.Add( new InputChangeHandlerWrapper( POLL_MUTE_OnPush_5, false ) );
    POLL_INPUT.OnDigitalPush.Add( new InputChangeHandlerWrapper( POLL_INPUT_OnPush_6, false ) );
    for( uint i = 0; i < 3; i++ )
        INPUT_POLL[i+1].OnDigitalPush.Add( new InputChangeHandlerWrapper( INPUT_POLL_OnPush_7, false ) );
        
    VOLUME_UP.OnDigitalPush.Add( new InputChangeHandlerWrapper( VOLUME_UP_OnPush_8, false ) );
    VOLUME_DOWN.OnDigitalPush.Add( new InputChangeHandlerWrapper( VOLUME_DOWN_OnPush_9, false ) );
    VOLUME_UP.OnDigitalRelease.Add( new InputChangeHandlerWrapper( VOLUME_UP_OnRelease_10, false ) );
    VOLUME_DOWN.OnDigitalRelease.Add( new InputChangeHandlerWrapper( VOLUME_UP_OnRelease_10, false ) );
    MUTE_ON.OnDigitalPush.Add( new InputChangeHandlerWrapper( MUTE_ON_OnPush_11, false ) );
    MUTE_OFF.OnDigitalPush.Add( new InputChangeHandlerWrapper( MUTE_OFF_OnPush_12, false ) );
    MUTE_TOGGLE.OnDigitalPush.Add( new InputChangeHandlerWrapper( MUTE_TOGGLE_OnPush_13, false ) );
    MUTE_TOGGLE.OnDigitalRelease.Add( new InputChangeHandlerWrapper( MUTE_TOGGLE_OnRelease_14, false ) );
    
    _SplusNVRAM.PopulateCustomAttributeList( true );
    
    NVRAM = _SplusNVRAM;
    
}

public override void LogosSimplSharpInitialize()
{
    MYDISPLAY  = new NewLineSTV.DisplayController();
    
    
}

public UserModuleClass_NEWLINERS232 ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}




const uint POWER_ON__DigitalInput__ = 0;
const uint POWER_OFF__DigitalInput__ = 1;
const uint POLL_POWER__DigitalInput__ = 2;
const uint POLL_VOLUME__DigitalInput__ = 3;
const uint POLL_MUTE__DigitalInput__ = 4;
const uint POLL_INPUT__DigitalInput__ = 5;
const uint VOLUME_UP__DigitalInput__ = 6;
const uint VOLUME_DOWN__DigitalInput__ = 7;
const uint MUTE_ON__DigitalInput__ = 8;
const uint MUTE_OFF__DigitalInput__ = 9;
const uint MUTE_TOGGLE__DigitalInput__ = 10;
const uint INPUT_POLL__DigitalInput__ = 11;
const uint RX__DOLLAR____AnalogSerialInput__ = 0;
const uint IS_POWER_ON_FB__DigitalOutput__ = 0;
const uint IS_MUTED_FB__DigitalOutput__ = 1;
const uint CURRENT_VOLUME_OUT__AnalogSerialOutput__ = 0;
const uint CURRENT_INPUT_FB__AnalogSerialOutput__ = 1;
const uint TX__DOLLAR____AnalogSerialOutput__ = 2;

[SplusStructAttribute(-1, true, false)]
public class SplusNVRAM : SplusStructureBase
{

    public SplusNVRAM( SplusObject __caller__ ) : base( __caller__ ) {}
    
    
}

SplusNVRAM _SplusNVRAM = null;

public class __CEvent__ : CEvent
{
    public __CEvent__() {}
    public void Close() { base.Close(); }
    public int Reset() { return base.Reset() ? 1 : 0; }
    public int Set() { return base.Set() ? 1 : 0; }
    public int Wait( int timeOutInMs ) { return base.Wait( timeOutInMs ) ? 1 : 0; }
}
public class __CMutex__ : CMutex
{
    public __CMutex__() {}
    public void Close() { base.Close(); }
    public void ReleaseMutex() { base.ReleaseMutex(); }
    public int WaitForMutex() { return base.WaitForMutex() ? 1 : 0; }
}
 public int IsNull( object obj ){ return (obj == null) ? 1 : 0; }
}


}
