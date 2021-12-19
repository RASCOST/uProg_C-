using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace uPROG2
{
    /********************
     * +--------------+ *
     * | SPI PIN OUT  | *
     * +--------------+ *
     * | MISO  --> D0 | *
     * | MOSI  --> D1 | *
     * | CLK   --> D2 | *
     * | RESET --> D3 | *
     * +--------------+ *
     *******************/
    
    class SPI
    {
        private double halfPeriod = 0.0;
        private USB8Bit usb8bit;
        private byte[] output = { 0 };
        private byte clock = 0;

        Stopwatch stopwatch = new Stopwatch();

        public SPI( double frequency, USB8Bit usb8bit, byte[] initPins )
        {
            if( 12.0 > frequency )
                halfPeriod = ( frequency * 1000000.0 ) / 4.0; // low/high period > 2 fclk
            else
                halfPeriod = ( frequency * 1000000.0 ) / 6.0; // low/high period > 3 fclk

            this.usb8bit = usb8bit;

            //initialize outputs
            //output[0] &= 0x01;
            usb8bit.setData( initPins );
        }

        /// <summary>
        /// 
        /// </summary>
        private byte CLOCK
        {
            set { clock = value; }
            get { return clock; }
        }



        /// <summary>
        /// Clock for SPI BUS
        /// </summary>
        /// <param name="hp"> half period value </param>
        public void sck( double hp, byte[] data )
        {
            byte logic = 0;

            while ( ( ( 1e6 * stopwatch.ElapsedTicks ) / Stopwatch.Frequency ) < hp );

            logic = ( byte )( data[ 0 ] & 0xFB );

            if (1 == CLOCK)
            {
                //OUTPUT = ( byte )( logic );
                CLOCK = 0;
            }
            else
            {
                data[ 0 ] |= ( byte )( logic | 0xF4 );
                CLOCK = 1;
            }

            usb8bit.setData( data );
        }

        /// <summary>
        /// Send data on the SPI BUS
        /// </summary>
        /// <param name="data"> data to be send </param>
        public void sendSPI( byte[] data )
        {
            byte[] temp = { 0 };
            byte[] temp2 = { 0 };

            for ( int i = 0; i < 7; i++ )
            {
                temp[ 0 ] = data[ 0 ];

                if( 0x80 == ( temp[0] & 0x80 ) << i )
                {
                    temp2[ 0 ] = ( byte )( data[0] & 0xFD );
                    data[ 0 ] = ( byte )( temp2[0] | 0x02 );
                }
                else
                    data[ 0 ] &= 0xFD;

                usb8bit.setData( data );
                sck( halfPeriod, data );
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte receiveSPI( byte[] data )
        {
            byte rec = 0;
            byte temp = 0;

            for( int i = 0; i < 7; i++ )
            {
                sck( halfPeriod, data );

                temp = usb8bit.getData();
                temp &= 0x01;

                if( 1 == temp )
                {
                    rec |= (byte)( 1 << (7 - i) );
                }
            }

            return rec;
        }
    }
}
