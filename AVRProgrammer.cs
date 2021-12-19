using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using uPROG2;
using System.Diagnostics;
using System.IO;

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

    class AVRProgrammer
    {
        public delegate void consoleEventArgsHandler(AVRProgrammer avr, WriteConsoleEvent wce);
        public event consoleEventArgsHandler EventConsole;

        //private SerialPortCom serial =new SerialPortCom();
        private USB8Bit usb8bit;
        private SupportedDevices device;
        private Timer timeCon = new Timer();
        private bool timeOut = true;
        private bool isBusy = true;
        private bool isClockHigh = false;
        public string console = string.Empty;

        Stopwatch stopwatch = new Stopwatch();

        #region Constructor

        public AVRProgrammer( USB8Bit usb8bit, SupportedDevices device )
        {
            //serial = serialPort;
            //serial.openSerialPortCom();
            this.usb8bit = usb8bit;
            this.device = device;
            timeCon.Elapsed += new ElapsedEventHandler( timeCon_Elapsed );
            timeCon.AutoReset = false;
            //timeCon.Enabled = true;
        }

        ~AVRProgrammer()
        {
            //serial.closeSerialPortCom();
        }

        #endregion

        #region Methods

        private bool Busy
        {
            set{ isBusy = value;}
            get{ return isBusy; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void timeCon_Elapsed(object source, ElapsedEventArgs e)
        {
            timeCon.Stop();
            timeOut = true;
            //Show Message ERROR
        }

        /// <summary>
        /// 
        /// </summary>
        private void onConsoleEvent()
        {
            if( null != EventConsole )
            {
                WriteConsoleEvent wce = new WriteConsoleEvent();
                wce.NewDataConsole = console;
                EventConsole( this, wce );
            }
        }

        /// <summary>
        /// Clock for SPI BUS
        /// </summary>
        /// <param name="hp"> half period value </param>
        public void sck(ref byte[] data)
        {
            stopwatch.Start();
            while (((1e6 * stopwatch.ElapsedTicks) / Stopwatch.Frequency) < 10) ;
            stopwatch.Stop();

            if ( isClockHigh )
            {
                data[0] = (byte)(data[0] & 0xFB);
                isClockHigh = false;
            }
            else
            {
                
                data[0] = (byte)((data[0] & 0xFB) | 0x04);
                isClockHigh = true;
            }

            usb8bit.setData(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool syncDevice()
        {
            byte[] init = { 0x08 };
            byte[] programEnable = { 0xAC, 0x53, 0x00, 0x00 };
            byte[] output = { 0 };
            byte temp = 0;
            byte rec = 0;
            byte aux = 0;

            consoleEvent("Synchronizing Device...\r\n");

            //initial sequence
            usb8bit.setData( init );
            timeCon.Interval = 1;
            timeCon.Start();
            while ( !timeOut ) ;
            init[ 0 ] = 0x00;
            usb8bit.setData( init );
            timeCon.Interval = 20;
            timeCon.Start();
            while ( !timeOut );

            //Program Enable Command
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    temp = (byte)( programEnable[i] << j );
                    output[0] = usb8bit.getData();

                    if ( 0x80 == ( 0x80 & temp ) )
                    {
                        output[0] = (byte)(output[0] & 0xFD);
                        output[0] = (byte)( output[0] | 0x02 );
                    }
                    else
                        output[0] = (byte)(0xFD & output[0]);

                    usb8bit.setData(output);
                    sck( ref output);

                    if ( i == 2 )
                    {
                        aux = (byte)( usb8bit.getData() & 0x01 );

                        if ( 1 == aux )
                            rec |= (byte)( 1 << ( 7 - j ) );
                    }

                    sck( ref output );
                }
            }

            if (0X53 == rec)
            {
                //isSync = true;
                return true;
            }
            else
            {
                //isSync = false;
                return false;
            }
        }

        public byte[] readSignatureByte()
        {
            byte[] readSigByte = { 0x30, 0x00, 0x00, 0x00 };
            byte[] signatureByte = { 0, 0, 0 };
            byte[] output = { 0 };
            byte temp = 0;
            byte aux = 0;
            byte rec = 0;

            for (byte address = 0; address < 3; address++)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        temp = (byte)(readSigByte[i] << j);
                        output[0] = usb8bit.getData();

                        if (0x80 == (0x80 & temp))
                        {
                            output[0] = (byte)(output[0] & 0xFD);
                            output[0] = (byte)(output[0] | 0x02);
                        }
                        else
                            output[0] = (byte)(0xFD & output[0]);

                        usb8bit.setData(output);
                        sck(ref output);

                        if (i == 3)
                        {
                            aux = (byte)(usb8bit.getData() & 0x01);

                            if (1 == aux)
                                rec |= (byte)(1 << (7 - j));
                        }

                        sck(ref output);
                    }
                }
                readSigByte[2] = (byte)(address + 1);
                signatureByte[ address ] = rec;
                rec = 0;
            }

            return signatureByte;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AVRData"></param>
        public void consoleEvent(string newConsoleData)
        {
            console = newConsoleData;
            onConsoleEvent();
        }

        /// <summary>
        /// 
        /// </summary>
        public void readFlash()
        {
            byte[] high = { 0X28, 0X00, 0X00, 0X00 };
            byte[] low = { 0X20, 0X00, 0X00, 0X00 };
            byte lowByte = 0;
            byte highByte = 0;
            byte temp = 0;
            byte aux = 0;
            byte rec = 0;
            byte[] output = { 0 };
            int address = 0;

            if (File.Exists("verify.txt"))
                File.Delete("verify.txt");

            FileStream fileVerify = new FileStream("verify.txt", FileMode.Create, FileAccess.Write);            

            try
            {
                StreamWriter verify = new StreamWriter(fileVerify, Encoding.ASCII);

                while (address < (device.flashNumPages * device.flashPageSize))
                {
                    low[2] = (byte)address;
                    low[1] = (byte)(address >> 8);

                    //read low byte
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            temp = (byte)(low[i] << j);
                            output[0] = usb8bit.getData();

                            if (0x80 == (0x80 & temp))
                            {
                                output[0] = (byte)(output[0] & 0xFD);
                                output[0] = (byte)(output[0] | 0x02);
                            }
                            else
                                output[0] = (byte)(0xFD & output[0]);

                            usb8bit.setData(output);
                            sck(ref output);

                            if (i == 3)
                            {
                                aux = (byte)(usb8bit.getData() & 0x01);

                                if (1 == aux)
                                    rec |= (byte)(1 << (7 - j));
                            }

                            sck(ref output);
                        }
                    }
                    lowByte = (byte)rec;
                    //consoleEvent(Convert.ToString(lowByte, 16) + "\t");
                    rec = 0;
                    //end read low byte

                    high[2] = (byte)address;
                    high[1] = (byte)(address >> 8);
                    //read high byte
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            temp = (byte)(high[i] << j);
                            output[0] = usb8bit.getData();

                            if (0x80 == (0x80 & temp))
                            {
                                output[0] = (byte)(output[0] & 0xFD);
                                output[0] = (byte)(output[0] | 0x02);
                            }
                            else
                                output[0] = (byte)(0xFD & output[0]);

                            usb8bit.setData(output);
                            sck(ref output);

                            if (i == 3)
                            {
                                aux = (byte)(usb8bit.getData() & 0x01);

                                if (1 == aux)
                                    rec |= (byte)(1 << (7 - j));
                            }

                            sck(ref output);
                        }
                    }
                    highByte = (byte)rec;
                    rec = 0;
                    consoleEvent(address.ToString() + "\t" + Convert.ToString(lowByte, 16).ToUpper() + "\t" + Convert.ToString(highByte, 16).ToUpper());
                    //end read high byte

                    if (0XFF == lowByte && 0XFF == highByte)
                    {
                        consoleEvent("Bytes readed: " + (address*2));
                        break;
                    }
                    
                    verify.WriteLine(Convert.ToString(lowByte, 16).ToUpper() + "\t" + Convert.ToString(highByte, 16).ToUpper());
                    address++;
                }
                verify.Close();
                fileVerify.Close();
            }
            catch (Exception e)
            {
                consoleEvent("File ERROR: " + e.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int readFlashAddress(int add, string d1, string d2)
        {
            byte[] low = { 0X20, 0X00, 0X00, 0X00 };
            byte temp = 0;
            byte aux = 0;
            byte rec = 0;
            byte[] output = { 0 };
            int address = 0;

            if (d1=="FF" || d2=="FF")
                address = add - 1;
            else
                address = add;

            low[2] = (byte)address;
            low[1] = (byte)(address >> 8);
            //read low byte
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    temp = (byte)(low[i] << j);
                    output[0] = usb8bit.getData();

                    if (0x80 == (0x80 & temp))
                    {
                        output[0] = (byte)(output[0] & 0xFD);
                        output[0] = (byte)(output[0] | 0x02);
                    }
                    else
                        output[0] = (byte)(0xFD & output[0]);

                    usb8bit.setData(output);
                    sck(ref output);

                    if (i == 3)
                    {
                        aux = (byte)(usb8bit.getData() & 0x01);

                        if (1 == aux)
                            rec |= (byte)(1 << (7 - j));
                    }

                    sck(ref output);
                }
            }

            return rec;
        }

        /// <summary>
        /// 
        /// </summary>
        public void writeFlash()
        {
            //Instructions
            byte[] WritePage = { 0x4C, 0x00, 0x00, 0x00 };
            byte[] LoadLowByte = { 0x40, 0x00, 0x00, 0x00 };
            byte[] LoadHighByte = { 0x48, 0x00, 0x00, 0x00 };
            byte[] busy = { 0xF0, 0x00, 0x00, 0x00 };
            //Variables
            byte[] output = { 0 }; //data to be write on SPI BUS
            byte temp = 0; //contains instruction shifted
            byte low = 0;  //low byte
            byte high = 0; //high byte
            byte aux = 0;
            byte rec = 0;
            int address = 0;
            byte size = (byte)(Math.Log10(device.flashPageSize) / Math.Log10(2));
            string data;   //data readed from file to be write in console
            string data1, data2; //data1: high byte, data2; low byte
            //File I/O
            FileStream input = new FileStream("flash.txt", FileMode.Open, FileAccess.Read);

            //Read data from file
            StreamReader sr = new StreamReader(input, Encoding.ASCII);

            while (true)
            {
                data = sr.ReadLine();

                //verify if we have finished to read the file
                if (data == null)
                    break;

                //get low byte
                data1 = data.Substring(0, 2);
                //get high byte
                data2 = data.Substring(3, 2);
                //show on console data from file

                //update address in the instruction
                LoadLowByte[2] = (byte) address;
                LoadLowByte[3] = (byte)Convert.ToByte(data1, 16);
                //consoleEvent(data1.ToString());

                //send low byte
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        temp = (byte)(LoadLowByte[i] << j);
                        output[0] = usb8bit.getData();

                        if (0x80 == (0x80 & temp))
                        {
                            output[0] = (byte)(output[0] & 0xFD);
                            output[0] = (byte)(output[0] | 0x02);
                        }
                        else
                            output[0] = (byte)(0xFD & output[0]);

                        usb8bit.setData(output);
                        //clock high enable slave to read data on SPI BUS
                        sck(ref output);
                        //clock low enable master to write data on SPI BUS
                        sck(ref output);
                    }
                }
                //end send low byte

                LoadHighByte[2] = (byte)address;
                LoadHighByte[3] = (byte)Convert.ToByte(data2, 16);
                consoleEvent(address.ToString() + "\t" + data1.ToString() + "\t" + data2.ToString());

                //send high byte
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        temp = (byte)(LoadHighByte[i] << j);
                        output[0] = usb8bit.getData();

                        if (0x80 == (0x80 & temp))
                        {
                            output[0] = (byte)(output[0] & 0xFD);
                            output[0] = (byte)(output[0] | 0x02);
                        }
                        else
                            output[0] = (byte)(0xFD & output[0]);

                        usb8bit.setData(output);
                        //clock high enable slave to read data on SPI BUS
                        sck(ref output);
                        //clock low enable master to write data on SPI BUS
                        sck(ref output);
                    }
                }
                //end send high byte

                //update progress bar

                if ((device.flashPageSize - 1) == (byte)(address & (device.flashPageSize - 1)))
                {
                    //line = 0;
                    low = (byte)address;
                    //low = (byte)(low & 0X03);
                    //low = (byte)((low << 6) & 0XC0);
                    high = (byte)(address >> 8);
                    //high = (byte)((high >> 2) & 0X3F);
                    WritePage[1] = high;
                    WritePage[2] = low;

                    //write page
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            temp = (byte)(WritePage[i] << j);
                            output[0] = usb8bit.getData();

                            if (0x80 == (0x80 & temp))
                            {
                                output[0] = (byte)(output[0] & 0xFD);
                                output[0] = (byte)(output[0] | 0x02);
                            }
                            else
                                output[0] = (byte)(0xFD & output[0]);

                            usb8bit.setData(output);
                            //clock high enable slave to read data on SPI BUS
                            sck(ref output);
                            //clock low enable master to write data on SPI BUS
                            sck(ref output);
                        }
                    }
                    //end write page

                    //wait until the page was not programmed
                    if (device.pollingCmd)
                    {
                        while (isBusy)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                for (int j = 0; j < 8; j++)
                                {
                                    temp = (byte)(busy[i] << j);
                                    output[0] = usb8bit.getData();

                                    if (0x80 == (0x80 & temp))
                                    {
                                        output[0] = (byte)(output[0] & 0xFD);
                                        output[0] = (byte)(output[0] | 0x02);
                                    }
                                    else
                                        output[0] = (byte)(0xFD & output[0]);

                                    usb8bit.setData(output);
                                    sck(ref output);

                                    if (i == 3)
                                    {
                                        aux = (byte)(usb8bit.getData() & 0x01);

                                        if (1 == aux)
                                            rec |= (byte)(1 << (7 - j));
                                    }

                                    sck(ref output);
                                }
                            }

                            if (0 == rec)
                            {
                                Busy = true;
                                rec = 0;
                            }
                            else
                                Busy = false;
                        }
                        Busy = true;
                    }
                    else
                    {
                        while (isBusy)
                        {
                            if (readFlashAddress(address, data1, data2) == 0xFF)
                                Busy = true;
                            else
                                Busy = false;
                        }
                        Busy = true;
                    }
                    //page++;
                    address++;
                    consoleEvent("Page: " + (address >> size).ToString() + "\r\n");
                }
                else
                    //line++;
                    address++;
            }
            //Because line++ is the last instruction
            //line--;
            address--;

            if ((device.flashPageSize - 1) != (byte)(address & (device.flashPageSize - 1)))
            {
                low = (byte)address;
                //low = (byte)(low & 0X03);
                //low = (byte)((low << 6) & 0XC0);
                high = (byte)(address >> 8);
                //high = (byte)((high >> 2) & 0X3F);

                WritePage[1] = high;
                WritePage[2] = low;

                //write page
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        temp = (byte)(WritePage[i] << j);
                        output[0] = usb8bit.getData();

                        if (0x80 == (0x80 & temp))
                        {
                            output[0] = (byte)(output[0] & 0xFD);
                            output[0] = (byte)(output[0] | 0x02);
                        }
                        else
                            output[0] = (byte)(0xFD & output[0]);

                        usb8bit.setData(output);
                        //clock high enable slave to read data on SPI BUS
                        sck(ref output);
                        //clock low enable master to write data on SPI BUS
                        sck(ref output);
                    }
                }
                //end write page

                //progressBar.Visible = false;
                consoleEvent("Memory page programmed: " + (address>>size).ToString() + "\r\n");
                sr.Close();
                input.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void erase()
        {
            //Instructions
            byte[] erase = { 0xAC, 0x80, 0x00, 0x00 };
            byte temp = 0;
            byte[] output = { 0 };

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    temp = (byte)(erase[i] << j);
                    output[0] = usb8bit.getData();

                    if (0x80 == (0x80 & temp))
                    {
                        output[0] = (byte)(output[0] & 0xFD);
                        output[0] = (byte)(output[0] | 0x02);
                    }
                    else
                        output[0] = (byte)(0xFD & output[0]);

                    usb8bit.setData(output);
                    //clock high enable slave to read data on SPI BUS
                    sck(ref output);
                    //clock low enable master to write data on SPI BUS
                    sck(ref output);
                }
            }
            consoleEvent("Flash erased");
        }

        /// <summary>
        /// 
        /// </summary>
        public void verifyFlash()
        {

            FileStream fileVerify = new FileStream("verify.txt", FileMode.Open, FileAccess.Read);
            FileStream fileFlash = new FileStream("flash.txt", FileMode.Open, FileAccess.Read);

            try
            {
                StreamReader verify = new StreamReader(fileVerify, Encoding.ASCII);
                StreamReader flash = new StreamReader(fileFlash, Encoding.ASCII);

                //if (fileVerify.Length != fileFlash.Length)
                //{
                //    consoleEvent("Verify error: program size don't match with file size");
                //    return;
                //}

                while (flash.ReadLine() == null)
                {
                    if( !flash.ReadLine().Equals( verify.ReadLine() ) )
                        consoleEvent("Flash don't match with file: " + flash.ReadLine() + "!=" + verify.ReadLine());
                }

                File.Delete("verify.txt");
                File.Delete("flash.txt");
            }
            catch(Exception e)
            {
                consoleEvent("ERROR while reading files to verify: " + e.ToString());
            }
        }
        #endregion
    }
}