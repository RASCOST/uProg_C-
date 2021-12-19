using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace uPROG2
{
    class SerialPortCom
    {
        private SerialPort serialPort = new SerialPort();

        /*#region SerialPortCom Properties
        /// <summary>
        ///
        /// </summary>
        public string BaudRate
        {
            get { return baudRate; }
            set { baudRate = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public string Parity
        {
            get { return parity; }
            set { parity = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string StopBits
        {
            get { return stopBits; }
            set { stopBits = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public string DataBits
        {
            get { return dataBits; }
            set { dataBits = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PortName
        {
            get { return portName; }
            set { portName = value; }
        }
        #endregion*/

        #region SerialPortCom Constructor
        ///<summary>
        /// Constructor with a defined BaudRate, Parity, StopBits, DataBits, Name.
        ///</summary>
        public SerialPortCom(int baud, Parity parity, StopBits sBits, int dBits, string name)
        {
            serialPort.BaudRate = baud;
            serialPort.Parity = parity;
            serialPort.StopBits = sBits;
            serialPort.DataBits = dBits;
            serialPort.PortName = name;
        }
        
        ///<summary>
        /// Default Constructor.
        ///</summary>
        public SerialPortCom()
        {
            serialPort.BaudRate = 57600;
            serialPort.Parity = System.IO.Ports.Parity.None;
            serialPort.StopBits = System.IO.Ports.StopBits.One;
            serialPort.DataBits = 8;
            serialPort.PortName = "COM1";
            serialPort.ReadTimeout = 5000;
        }

        #endregion

        #region Methods
        ///<summary>
        /// Open SerialPort
        ///</summary>
        public Boolean openSerialPortCom()
        {

            if (!serialPort.IsOpen)
            {
                serialPort.Open();
                return true;
            }
            else
            {
                return false;
            }
        }

        ///<summary>
        /// Close SerialPort
        ///</summary>
        public void closeSerialPortCom()
        {
            serialPort.Close();
        }

        ///<summary>
        /// Write SerialPort
        ///</summary>
        public void writeSerialPortCom(byte[] data, int ini, int end)
        {
            serialPort.Write(data, ini, end);
            serialPort.DiscardInBuffer();
        }

        ///<summary>
        /// Read SerialPort
        ///</summary>
        public int readSerialPortCom(ref bool timeOut)
        {
            try
            {
                timeOut = false;
                return serialPort.ReadByte();
            }
            catch(TimeoutException) 
            {
                timeOut = true;
                return -1;
            }

        }
        #endregion
    }
}
