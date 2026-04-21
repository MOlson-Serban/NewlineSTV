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
        Crestron.Logos.SplusObjects.DigitalInput POLL_INPUT;
        Crestron.Logos.SplusObjects.DigitalInput POLL_VOLUME;
        Crestron.Logos.SplusObjects.DigitalInput SET_VOLUME_100;
        InOutArray<Crestron.Logos.SplusObjects.DigitalInput> INPUT_POLL;
        Crestron.Logos.SplusObjects.StringInput RX__DOLLAR__;
        Crestron.Logos.SplusObjects.DigitalOutput IS_POWER_ON_FB;
        Crestron.Logos.SplusObjects.AnalogOutput CURRENT_INPUT_FB;
        Crestron.Logos.SplusObjects.AnalogOutput CURRENT_VOLUME_OUT;
        Crestron.Logos.SplusObjects.StringOutput TX__DOLLAR__;
        NewLineSTV.DisplayController MYDISPLAY;
        public void ONPOWERCALLBACK ( ushort STATE ) 
            { 
            try
            {
                SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
                
                __context__.SourceCodeLine = 28;
                IS_POWER_ON_FB  .Value = (ushort) ( STATE ) ; 
                
                
            }
            finally { ObjectFinallyHandler(); }
            }
            
        public void ONINPUTCALLBACK ( ushort VAL ) 
            { 
            try
            {
                SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
                
                __context__.SourceCodeLine = 29;
                CURRENT_INPUT_FB  .Value = (ushort) ( VAL ) ; 
                
                
            }
            finally { ObjectFinallyHandler(); }
            }
            
        public void ONVOLUMECALLBACK ( ushort VAL ) 
            { 
            try
            {
                SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
                
                __context__.SourceCodeLine = 30;
                CURRENT_VOLUME_OUT  .Value = (ushort) ( VAL ) ; 
                
                
            }
            finally { ObjectFinallyHandler(); }
            }
            
        public void ONMUTECALLBACK ( ushort STATE ) 
            { 
            try
            {
                SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
                
                
                
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
                
                __context__.SourceCodeLine = 39;
                RAWBYTES__DOLLAR__  =  ( ""  )  .ToString() ; 
                __context__.SourceCodeLine = 41;
                ushort __FN_FORSTART_VAL__1 = (ushort) ( 1 ) ;
                ushort __FN_FOREND_VAL__1 = (ushort)Functions.Length( HEXDATA .ToString() .ToString() ); 
                int __FN_FORSTEP_VAL__1 = (int)2; 
                for ( I  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (I  >= __FN_FORSTART_VAL__1) && (I  <= __FN_FOREND_VAL__1) ) : ( (I  <= __FN_FORSTART_VAL__1) && (I  >= __FN_FOREND_VAL__1) ) ; I  += (ushort)__FN_FORSTEP_VAL__1) 
                    { 
                    __context__.SourceCodeLine = 43;
                    RAWBYTES__DOLLAR__  =  ( RAWBYTES__DOLLAR__ + Functions.Chr (  (int) ( Functions.HextoI( Functions.Mid( HEXDATA .ToString() , (int)( I ) , (int)( 2 ) ) ) ) )  )  .ToString() ; 
                    __context__.SourceCodeLine = 41;
                    } 
                
                __context__.SourceCodeLine = 45;
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
                
                
                __context__.SourceCodeLine = 53;
                HEXSTR__DOLLAR__  .UpdateValue ( ""  ) ; 
                __context__.SourceCodeLine = 55;
                ushort __FN_FORSTART_VAL__1 = (ushort) ( 1 ) ;
                ushort __FN_FOREND_VAL__1 = (ushort)Functions.Length( RX__DOLLAR__ ); 
                int __FN_FORSTEP_VAL__1 = (int)1; 
                for ( I  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (I  >= __FN_FORSTART_VAL__1) && (I  <= __FN_FOREND_VAL__1) ) : ( (I  <= __FN_FORSTART_VAL__1) && (I  >= __FN_FOREND_VAL__1) ) ; I  += (ushort)__FN_FORSTEP_VAL__1) 
                    { 
                    __context__.SourceCodeLine = 57;
                    B = (ushort) ( Byte( RX__DOLLAR__ , (int)( I ) ) ) ; 
                    __context__.SourceCodeLine = 58;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( B < 16 ))  ) ) 
                        { 
                        __context__.SourceCodeLine = 60;
                        HEXSTR__DOLLAR__  .UpdateValue ( HEXSTR__DOLLAR__ + "0" + Functions.ItoHex (  (int) ( B ) )  ) ; 
                        } 
                    
                    else 
                        { 
                        __context__.SourceCodeLine = 64;
                        HEXSTR__DOLLAR__  .UpdateValue ( HEXSTR__DOLLAR__ + Functions.ItoHex (  (int) ( B ) )  ) ; 
                        } 
                    
                    __context__.SourceCodeLine = 55;
                    } 
                
                __context__.SourceCodeLine = 68;
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
            
            __context__.SourceCodeLine = 71;
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
        
        __context__.SourceCodeLine = 72;
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
        
        __context__.SourceCodeLine = 73;
        MYDISPLAY . PollPower ( ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object POLL_INPUT_OnPush_4 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 74;
        MYDISPLAY . PollInput ( ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object POLL_VOLUME_OnPush_5 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 75;
        MYDISPLAY . PollVolume ( ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object SET_VOLUME_100_OnPush_6 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 78;
        MYDISPLAY . SetVolume ( (ushort)( 100 )) ; 
        
        
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
        
        
        __context__.SourceCodeLine = 83;
        I = (ushort) ( Functions.GetLastModifiedArrayIndex( __SignalEventArg__ ) ) ; 
        __context__.SourceCodeLine = 84;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( I > 0 ) ) && Functions.TestForTrue ( Functions.BoolToInt ( I <= 3 ) )) ))  ) ) 
            { 
            __context__.SourceCodeLine = 84;
            MYDISPLAY . SetInput ( (ushort)( I )) ; 
            } 
        
        
        
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
        
        __context__.SourceCodeLine = 90;
        WaitForInitializationComplete ( ) ; 
        __context__.SourceCodeLine = 92;
        // RegisterDelegate( MYDISPLAY , ONPOWERCHANGE , ONPOWERCALLBACK ) 
        MYDISPLAY .OnPowerChange  = ONPOWERCALLBACK; ; 
        __context__.SourceCodeLine = 93;
        // RegisterDelegate( MYDISPLAY , ONINPUTCHANGE , ONINPUTCALLBACK ) 
        MYDISPLAY .OnInputChange  = ONINPUTCALLBACK; ; 
        __context__.SourceCodeLine = 94;
        // RegisterDelegate( MYDISPLAY , ONVOLUMECHANGE , ONVOLUMECALLBACK ) 
        MYDISPLAY .OnVolumeChange  = ONVOLUMECALLBACK; ; 
        __context__.SourceCodeLine = 95;
        // RegisterDelegate( MYDISPLAY , ONMUTECHANGE , ONMUTECALLBACK ) 
        MYDISPLAY .OnMuteChange  = ONMUTECALLBACK; ; 
        __context__.SourceCodeLine = 96;
        // RegisterDelegate( MYDISPLAY , TRANSMITSERIALDATA , ONTRANSMITSERIAL ) 
        MYDISPLAY .TransmitSerialData  = ONTRANSMITSERIAL; ; 
        __context__.SourceCodeLine = 98;
        MYDISPLAY . PollPower ( ) ; 
        __context__.SourceCodeLine = 99;
        MYDISPLAY . PollInput ( ) ; 
        __context__.SourceCodeLine = 100;
        MYDISPLAY . PollVolume ( ) ; 
        
        
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
    
    POLL_INPUT = new Crestron.Logos.SplusObjects.DigitalInput( POLL_INPUT__DigitalInput__, this );
    m_DigitalInputList.Add( POLL_INPUT__DigitalInput__, POLL_INPUT );
    
    POLL_VOLUME = new Crestron.Logos.SplusObjects.DigitalInput( POLL_VOLUME__DigitalInput__, this );
    m_DigitalInputList.Add( POLL_VOLUME__DigitalInput__, POLL_VOLUME );
    
    SET_VOLUME_100 = new Crestron.Logos.SplusObjects.DigitalInput( SET_VOLUME_100__DigitalInput__, this );
    m_DigitalInputList.Add( SET_VOLUME_100__DigitalInput__, SET_VOLUME_100 );
    
    INPUT_POLL = new InOutArray<DigitalInput>( 3, this );
    for( uint i = 0; i < 3; i++ )
    {
        INPUT_POLL[i+1] = new Crestron.Logos.SplusObjects.DigitalInput( INPUT_POLL__DigitalInput__ + i, INPUT_POLL__DigitalInput__, this );
        m_DigitalInputList.Add( INPUT_POLL__DigitalInput__ + i, INPUT_POLL[i+1] );
    }
    
    IS_POWER_ON_FB = new Crestron.Logos.SplusObjects.DigitalOutput( IS_POWER_ON_FB__DigitalOutput__, this );
    m_DigitalOutputList.Add( IS_POWER_ON_FB__DigitalOutput__, IS_POWER_ON_FB );
    
    CURRENT_INPUT_FB = new Crestron.Logos.SplusObjects.AnalogOutput( CURRENT_INPUT_FB__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( CURRENT_INPUT_FB__AnalogSerialOutput__, CURRENT_INPUT_FB );
    
    CURRENT_VOLUME_OUT = new Crestron.Logos.SplusObjects.AnalogOutput( CURRENT_VOLUME_OUT__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( CURRENT_VOLUME_OUT__AnalogSerialOutput__, CURRENT_VOLUME_OUT );
    
    RX__DOLLAR__ = new Crestron.Logos.SplusObjects.StringInput( RX__DOLLAR____AnalogSerialInput__, 1024, this );
    m_StringInputList.Add( RX__DOLLAR____AnalogSerialInput__, RX__DOLLAR__ );
    
    TX__DOLLAR__ = new Crestron.Logos.SplusObjects.StringOutput( TX__DOLLAR____AnalogSerialOutput__, this );
    m_StringOutputList.Add( TX__DOLLAR____AnalogSerialOutput__, TX__DOLLAR__ );
    
    
    RX__DOLLAR__.OnSerialChange.Add( new InputChangeHandlerWrapper( RX__DOLLAR___OnChange_0, false ) );
    POWER_ON.OnDigitalPush.Add( new InputChangeHandlerWrapper( POWER_ON_OnPush_1, false ) );
    POWER_OFF.OnDigitalPush.Add( new InputChangeHandlerWrapper( POWER_OFF_OnPush_2, false ) );
    POLL_POWER.OnDigitalPush.Add( new InputChangeHandlerWrapper( POLL_POWER_OnPush_3, false ) );
    POLL_INPUT.OnDigitalPush.Add( new InputChangeHandlerWrapper( POLL_INPUT_OnPush_4, false ) );
    POLL_VOLUME.OnDigitalPush.Add( new InputChangeHandlerWrapper( POLL_VOLUME_OnPush_5, false ) );
    SET_VOLUME_100.OnDigitalPush.Add( new InputChangeHandlerWrapper( SET_VOLUME_100_OnPush_6, false ) );
    for( uint i = 0; i < 3; i++ )
        INPUT_POLL[i+1].OnDigitalPush.Add( new InputChangeHandlerWrapper( INPUT_POLL_OnPush_7, false ) );
        
    
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
const uint POLL_INPUT__DigitalInput__ = 3;
const uint POLL_VOLUME__DigitalInput__ = 4;
const uint SET_VOLUME_100__DigitalInput__ = 5;
const uint INPUT_POLL__DigitalInput__ = 6;
const uint RX__DOLLAR____AnalogSerialInput__ = 0;
const uint IS_POWER_ON_FB__DigitalOutput__ = 0;
const uint CURRENT_INPUT_FB__AnalogSerialOutput__ = 0;
const uint CURRENT_VOLUME_OUT__AnalogSerialOutput__ = 1;
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
