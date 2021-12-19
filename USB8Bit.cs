using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FTD2XX_NET;

namespace uPROG2
{
    class USB8Bit
    {
        /******************************
         * +------------------------+ *
         * |    FT232RL PIN OUT     | *
         * +------------------------+ *
         * | Pin 6  ( RI# )  --> D7 | *
         * | Pin 10 ( DCD# ) --> D6 | *
         * | Pin 9  ( DSR# ) --> D5 | *
         * | Pin 2  ( DTR# ) --> D4 | *
         * | Pin 11 ( CTS# ) --> D3 | *
         * | Pin 3  ( RTS# ) --> D2 | *
         * | Pin 5  ( RXD )  --> D1 | *
         * | Pin 1  ( TXD )  --> D0 | *
         * +------------------------+ *
         * ***************************/

        private FTDI.FT_STATUS ftStatus;
        //private byte mask = 0xFE;
        //private byte MODE = 1;
        //private uint BAUDRATE = 115200;

        public string usbString = string.Empty;

        public delegate void consoleEventArgsHandler( USB8Bit usb8bit, WriteConsoleEvent wce );
        public event consoleEventArgsHandler EventConsole;

        private FTDI usb = new FTDI();

        public USB8Bit(byte IO, byte MODE, uint BAUDRATE)
        {
            ftStatus = usb.OpenByDescription( "FT232R USB UART" );
            
            if ( FTDI.FT_STATUS.FT_OK == ftStatus )
                consoleEvent( "USB OPEN: OK" );
            else
                consoleEvent( "USB OPEN: FAILED" );

            ftStatus = usb.SetBaudRate(BAUDRATE);

            if ( FTDI.FT_STATUS.FT_OK == ftStatus )
                consoleEvent( "USB Baud Rate: " + BAUDRATE.ToString() );
            else
                consoleEvent( "USB Baud Rate: FAILED" );

            ftStatus = usb.SetBitMode( IO, MODE );

            if ( FTDI.FT_STATUS.FT_OK == ftStatus )
                consoleEvent( "USB Bit Mode: OK" );
            else
                consoleEvent( "USB Bit Mode: FAILED" );
        }

        ~USB8Bit()
        {
            closeUsb();
        }

        /// <summary>
        /// 
        /// </summary>
        private void onConsoleEvent()
        {
            if( null != EventConsole )
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
        public void consoleEvent(string newConsoleData)
        {
            usbString = newConsoleData;

            onConsoleEvent();
        }

        /// <summary>
        /// 
        /// </summary>
        //public byte MASK
        //{
        //    set{ mask = value; }
        //    get { return mask; }
        //}

        /// <summary>
        /// 
        /// </summary>
        private void closeUsb()
        {
            usb.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public void setData( byte[] output )
        {
            uint s = 0;

            usb.Write( output, output.Length, ref s );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte getData()
        {
            byte input = 0;

            usb.GetPinStates( ref input );

            return input;
        }
    }
}
