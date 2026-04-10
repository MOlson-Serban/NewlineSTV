namespace NewLineSTV;
        // class declarations
         class DisplayController;
     class DisplayController 
    {
        // class delegates
        delegate FUNCTION StateChangeHandler ( INTEGER state );
        delegate FUNCTION SerialDataHandler ( SIMPLSHARPSTRING data );

        // class events

        // class functions
        FUNCTION ReceiveHexData ( SIMPLSHARPSTRING rxHexData );
        FUNCTION PollPower ();
        FUNCTION PollInput ();
        FUNCTION PollVolume ();
        FUNCTION PollMute ();
        FUNCTION PowerOn ();
        FUNCTION PowerOff ();
        FUNCTION SetInput ( INTEGER inputVal );
        FUNCTION SetVolume ( INTEGER volVal );
        FUNCTION SetMute ( INTEGER muteState );
        FUNCTION ToggleMute ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();
        STRING_FUNCTION ToString ();

        // class variables
        INTEGER __class_id__;

        // class properties
        DelegateProperty StateChangeHandler OnPowerChange;
        DelegateProperty StateChangeHandler OnInputChange;
        DelegateProperty StateChangeHandler OnVolumeChange;
        DelegateProperty StateChangeHandler OnMuteChange;
        DelegateProperty SerialDataHandler TransmitSerialData;
    };

