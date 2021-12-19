using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FTD2XX_NET;

namespace uPROG2
{
    class USBCom
    {
        public delegate void consoleEventArgsHandler( USBCom usbCom, WriteConsoleEvent wce );
        public event consoleEventArgsHandler EventConsole;

        private FTDI usb = new FTDI();

        private UInt32 deviceCount = 0;
        private string usbString = string.Empty;

        //Status of the FTDI device
        FTDI.FT_STATUS status = FTDI.FT_STATUS.FT_OK;
        FTDI.FT_DEVICE_INFO_NODE[] deviceList;

        /// <summary>
        /// 
        /// </summary>
        public USBCom()
        {
            status = usb.SetDataCharacteristics( FTDI.FT_DATA_BITS.FT_BITS_8, FTDI.FT_STOP_BITS.FT_STOP_BITS_1, FTDI.FT_PARITY.FT_PARITY_NONE );

            if (FTDI.FT_STATUS.FT_OK != status)
            {
                consoleEvent("Set data characteristics error: " + status.ToString());
                return;
            }

            status = usb.SetBaudRate( 9600 );

            if (FTDI.FT_STATUS.FT_OK != status)
            {
                consoleEvent("Set baud rate error: " + status.ToString());
                return;
            }            
        }

        public void setFlowControl()
        {
            status = usb.SetFlowControl( FTDI.FT_FLOW_CONTROL.FT_FLOW_DTR_DSR, 0x11, 0x13 );
        }

        /// <summary>
        /// 
        /// </summary>
        private void onConsoleEvent()
        {
            if (null != EventConsole)
            {
                WriteConsoleEvent wce = new WriteConsoleEvent();
                wce.NewDataConsole = usbString;
                EventConsole(this, wce);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        public void consoleEvent( string s )
        {
            usbString = s;

            onConsoleEvent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private FTDI.FT_STATUS getFTDIDevices( ref UInt32 number )
        {
            status = usb.GetNumberOfDevices( ref number );

            return status;
        }

        /// <summary>
        /// 
        /// </summary>
        public void openUsb()
        {
            getFTDIDevices( ref deviceCount);
            //Info about FTDI device
            deviceList = new FTDI.FT_DEVICE_INFO_NODE[deviceCount];

            if (0 == deviceCount)
            {
                //enviar mensagem para consola
                consoleEvent( "No drivers were found for uPROG!" + status.ToString() );
                
                return;
            }
            else
            {
                usb.GetDeviceList(deviceList);
                status = usb.OpenBySerialNumber(deviceList[0].SerialNumber);

                if( FTDI.FT_STATUS.FT_DEVICE_NOT_OPENED == status )
                    consoleEvent("Can't open USB");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void closeUsb()
        {
            usb.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        public void setRts()
        {
            usb.SetRTS(true);
        }

        /// <summary>
        /// 
        /// </summary>
        public void clearRts()
        {
            usb.SetRTS(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte getCts()
        {
            byte cts = 0;

            usb.GetModemStatus (ref cts );

            if ( ( cts & 0x10 ) == 1 )
                return 1;
            else
                return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public void setDtr()
        {
            usb.SetDTR( true );
        }

        /// <summary>
        /// 
        /// </summary>
        public void clearDtr()
        {
            usb.SetDTR( false );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte getDsr()
        {
            byte dsr = 0;

            usb.GetModemStatus( ref dsr );

            if ( ( dsr & 0x20 ) == 1 )
                return 1;
            else
                return 0;
        }
    }
}
