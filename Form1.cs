using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FTD2XX_NET;
using System.IO.Ports;
using uPROG2;

namespace uPROG2
{
    enum Mode { SERIAL, USB, SPI };

    public partial class FormUPROG : Form
    {
        private string device= string.Empty;
        private bool synchronized = false;
        private bool fileLoaded = false;
        private bool isFrequencySet = false;
        private byte progMode = ( byte ) Mode.SPI;
        public double frequency = 0.0;

        private USB8Bit usb;
        private HexFile hex;
        public SupportedDevices[] supportedDevices = new SupportedDevices[4];
        public SupportedDevices deviceParameters = new SupportedDevices();
        private AVRProgrammer avr;

        #region GUI Constructor
        ///<summary>
        ///
        ///</summary>
        public FormUPROG()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        #endregion

        #region GUI Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormUPROG_Load(object sender, EventArgs e)
        {
            int index;

            loadSupportedDevices();

            FormSelDev selectionForm = new FormSelDev( this );       
            selectionForm.ShowDialog();

            usb = new USB8Bit( 0xFE, 1, 115000 );
            hex = new HexFile();

            for( index = 0; index < supportedDevices.Length; index++ )
                if ( supportedDevices[ index ].name.Equals( device ) )
                {
                    avr = new AVRProgrammer( usb, supportedDevices[ index ] );
                    deviceParameters = supportedDevices[ index ];
                    break;
                }

            avr.EventConsole += new AVRProgrammer.consoleEventArgsHandler(avrUpdateConsole);
            usb.EventConsole += new USB8Bit.consoleEventArgsHandler(usbUpdateConsole);

            //radioButtonSerial.Checked = true;
            radioButtonSPI.Checked = true;
            //loadSupportedDevices();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormUPROG_Leave(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Execute READ FLASH INSTRUCTION
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonReadFlash_Click(object sender, EventArgs e)
        {
            byte[] signatureByte;

            if( synchronized )
            {
                ///call READ Instructions
                richTextBoxConsole.AppendText(">>Reading Flash Memory:" + "\r\n");
                avr.readFlash();
            }
            else if( avr.syncDevice() )
            {
                synchronized = true;
                signatureByte = avr.readSignatureByte();
                

                if (signatureByte[0] != deviceParameters.signatureByte1)
                {
                    richTextBoxConsole.AppendText(">>Wrong device on board 1:" + signatureByte[0].ToString() + "\r\n");
                    return;
                }
                else if (signatureByte[1] != deviceParameters.signatureByte2)
                {
                    richTextBoxConsole.AppendText(">>Wrong device on board 2:" + signatureByte[1].ToString() + "\r\n");
                    return;
                }
                else if (signatureByte[2] != deviceParameters.signatureByte3)
                {
                    richTextBoxConsole.AppendText(">>Wrong device on board 3:" + signatureByte[2].ToString() + "\r\n");
                    return;
                }
                else
                {
                    richTextBoxConsole.AppendText(">>Device on board:"+ deviceParameters.name + "\r\n");
                    richTextBoxConsole.AppendText(">>Reading Flash Memory:" + "\r\n");
                    avr.readFlash();
                }
            }
            else
                MessageBox.Show("Can't connect with the board. Please verify if board is plugged-in , connections are OK and, power on!", "VERIFY BOARD", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWriteFlash_Click(object sender, EventArgs e)
        {
            byte[] signatureByte;

            //verify if there's a file to program
            if (!FileLoaded)
            {
                MessageBox.Show("You have to open an hex file!!!", "No File Loaded", MessageBoxButtons.OK);
                return;
            }

            if (synchronized)
            {
                //call ERASE Instruction
                richTextBoxConsole.AppendText(">>Erasing Flash Memory:" + "\r\n");
                //avr.erase();
                //call READ Instruction
                richTextBoxConsole.AppendText(">>Writing Flash Memory:" + "\r\n");
                avr.writeFlash();
            }
            else if (avr.syncDevice())
            {
                synchronized = true;
                signatureByte = avr.readSignatureByte();


                if (signatureByte[0] != deviceParameters.signatureByte1)
                {
                    richTextBoxConsole.AppendText(">>Wrong device on board 1:" + signatureByte[0].ToString() + "\r\n");
                    return;
                }
                else if (signatureByte[1] != deviceParameters.signatureByte2)
                {
                    richTextBoxConsole.AppendText(">>Wrong device on board 2:" + signatureByte[1].ToString() + "\r\n");
                    return;
                }
                else if (signatureByte[2] != deviceParameters.signatureByte3)
                {
                    richTextBoxConsole.AppendText(">>Wrong device on board 3:" + signatureByte[2].ToString() + "\r\n");
                    return;
                }
                else
                {
                    richTextBoxConsole.AppendText(">>Device on board:" + deviceParameters.name + "\r\n");
                    richTextBoxConsole.AppendText(">>Erasing Flash Memory:" + "\r\n");
                    avr.erase();
                    richTextBoxConsole.AppendText(">>Writing Flash Memory:" + "\r\n");
                    avr.writeFlash();
                    //avr.readFlash();
                    //avr.verifyFlash();
                }
            }
            else
                MessageBox.Show("Can't connect with the board. Please verify if board is plugged-in , connections are OK and, power on!", "VERIFY BOARD", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            FileLoaded = false;
            //avr.verifyFlash();
            //apagar ficheiro flash.txt
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEraseFlash_Click(object sender, EventArgs e)
        {
            byte[] signatureByte;

            if (synchronized)
            {
                ///call ERASE Instructions
                richTextBoxConsole.AppendText(">>Erasing Flash Memory:" + "\r\n");
                avr.erase();
            }
            else if (avr.syncDevice())
            {
                synchronized = true;
                signatureByte = avr.readSignatureByte();


                if (signatureByte[0] != deviceParameters.signatureByte1)
                {
                    richTextBoxConsole.AppendText(">>Wrong device on board 1:" + signatureByte[0].ToString() + "\r\n");
                    return;
                }
                else if (signatureByte[1] != deviceParameters.signatureByte2)
                {
                    richTextBoxConsole.AppendText(">>Wrong device on board 2:" + signatureByte[1].ToString() + "\r\n");
                    return;
                }
                else if (signatureByte[2] != deviceParameters.signatureByte3)
                {
                    richTextBoxConsole.AppendText(">>Wrong device on board 3:" + signatureByte[2].ToString() + "\r\n");
                    return;
                }
                else
                {
                    richTextBoxConsole.AppendText(">>Device on board:" + deviceParameters.name + "\r\n");
                    richTextBoxConsole.AppendText(">>Erasing Flash Memory:" + "\r\n");
                    avr.erase();
                }
            }
            else
                MessageBox.Show("Can't connect with the board. Please verify if board is plugged-in , connections are OK and, power on!", "VERIFY BOARD", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBrowser_Click(object sender, EventArgs e)
        {
            // Configure open file dialog box
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Title = "Select  HEX FILE";
            //dialog.FileName = "HEX file"; // Default file name
            dialog.DefaultExt = ".hex"; // Default file extension
            dialog.Filter = "HEX File (.hex)|*.hex"; // Filter files by extension



            if (DialogResult.Cancel != dialog.ShowDialog())
            {
                if (dialog.FileName == string.Empty)
                    MessageBox.Show("You must select a file", "OPEN HEX FILE", MessageBoxButtons.OK);
                else
                {
                    textBoxBrowser.Text = dialog.FileName;
                    hex.readHexFile(dialog.FileName);
                    richTextBoxConsole.AppendText(">>File:" + dialog.FileName + "\r\n");
                    richTextBoxConsole.AppendText(">>Size: " + Convert.ToString(hex.getFileSize()) + "bytes\r\n");
                    //permission to program flash
                    FileLoaded = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonSerial_CheckedChanged(object sender, EventArgs e)
        {
            progMode = (byte)Mode.SERIAL;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonSPI_CheckedChanged(object sender, EventArgs e)
        {
            progMode = (byte)Mode.SPI;
            textBoxFrequency.Enabled = true;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonUSB_CheckedChanged(object sender, EventArgs e)
        {
            progMode = (byte)Mode.USB;
            textBoxFrequency.Enabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxFrequency_TextChanged(object sender, EventArgs e)
        {
            if ( string.IsNullOrEmpty( textBoxFrequency.Text ) )
            {
                
                return;
            }
            else if (!double.TryParse(textBoxFrequency.Text, out frequency))
            {
                MessageBox.Show("Invalid Frequency, please try again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isFrequencySet = false;
                return;
            }
            else if (0 < frequency)
            {
                isFrequencySet = true;
                return;
            }
            else
            {
                isFrequencySet = false;
                MessageBox.Show("Frequency must be greater than zero!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void avrUpdateConsole(AVRProgrammer avr, WriteConsoleEvent wrc)
        {
            richTextBoxConsole.AppendText( ">>" + wrc.NewDataConsole + "\r\n" );
            richTextBoxConsole.ScrollToCaret();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usb"></param>
        /// <param name="wrc"></param>
        private void usbUpdateConsole(USB8Bit usb, WriteConsoleEvent wrc)
        {
            richTextBoxConsole.AppendText(">>" + wrc.NewDataConsole + "\r\n");
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        private string Device
        {
            set { device = value; }
            get { return device; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void getDevice(string name)
        {
            device = name;

            richTextBoxConsole.AppendText( ">>Selected device: " + device + "\r\n" );
        }
        
        /// <summary>
        /// 
        /// </summary>
        private bool Synchronized
        {
            set { synchronized = value; }

            get { return synchronized; }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool FileLoaded
        {
            set { fileLoaded = true; }

            get { return fileLoaded; }
        }

        /// <summary>
        /// 
        /// </summary>
        private byte ProgMode
        {
            set { progMode = value; }
            get { return progMode; }
        }
    
        /// <summary>
        /// 
        /// </summary>
        private void loadSupportedDevices()
        {
            //ATMEGA32
            supportedDevices[0].name = "Atmega32";
            supportedDevices[0].flash = 32;
            supportedDevices[0].flashPageSize = 64;
            supportedDevices[0].flashNumPages = 256;
            supportedDevices[0].eeprom = 1;
            supportedDevices[0].eepromPageSize = 4;
            supportedDevices[0].eepromNumPages = 256;
            supportedDevices[0].signatureByte1 = 0x1E;
            supportedDevices[0].signatureByte2 = 0x95;
            supportedDevices[0].signatureByte3 = 0x02;
            supportedDevices[0].sigByte1Ad = 0x00;
            supportedDevices[0].sigByte2Ad = 0x01;
            supportedDevices[0].sigByte3Ad = 0x02;
            supportedDevices[0].pollingCmd = false;

            //ATMEGA88
            supportedDevices[1].name = "Atmega88";
            supportedDevices[1].flash = 8;
            supportedDevices[1].flashPageSize = 32;
            supportedDevices[1].flashNumPages = 128;
            supportedDevices[1].eeprom = 512;
            supportedDevices[1].eepromPageSize = 4;
            supportedDevices[1].eepromNumPages = 128;
            supportedDevices[1].signatureByte1 = 0x1E;
            supportedDevices[1].signatureByte2 = 0x93;
            supportedDevices[1].signatureByte3 = 0x0A;
            supportedDevices[1].sigByte1Ad = 0x00;
            supportedDevices[1].sigByte2Ad = 0x01;
            supportedDevices[1].sigByte3Ad = 0x02;
            supportedDevices[1].pollingCmd = true;

            //ATMEGA8
            supportedDevices[2].name = "Atmega8";
            supportedDevices[2].flash = 8;
            supportedDevices[2].flashPageSize = 32;
            supportedDevices[2].flashNumPages = 128;
            supportedDevices[2].eeprom = 512;
            supportedDevices[2].eepromPageSize = 4;
            supportedDevices[2].eepromNumPages = 128;
            supportedDevices[2].signatureByte1 = 0x1E;
            supportedDevices[2].signatureByte2 = 0x93;
            supportedDevices[2].signatureByte3 = 0x07;
            supportedDevices[2].sigByte1Ad = 0x00;
            supportedDevices[2].sigByte2Ad = 0x01;
            supportedDevices[2].sigByte3Ad = 0x02;
            supportedDevices[2].pollingCmd = false;

            //ATtiny45
            supportedDevices[3].name = "ATtiny45";
            supportedDevices[3].flash = 4;
            supportedDevices[3].flashPageSize = 32;
            supportedDevices[3].flashNumPages = 64;
            supportedDevices[3].eeprom = 256;
            supportedDevices[3].eepromPageSize = 4;
            supportedDevices[3].eepromNumPages = 64;
            supportedDevices[3].signatureByte1 = 0x1E;
            supportedDevices[3].signatureByte2 = 0x92;
            supportedDevices[3].signatureByte3 = 0x06;
            supportedDevices[3].sigByte1Ad = 0x00;
            supportedDevices[3].sigByte2Ad = 0x02;
            supportedDevices[3].sigByte3Ad = 0x04;
            supportedDevices[3].pollingCmd = true;
        }

        #endregion
    }
}
