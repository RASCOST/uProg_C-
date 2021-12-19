using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace uPROG2
{
    class HexFile
    {
        public int size = 0;

        #region HexFile Cosntructor
        /// <summary>
        /// 
        /// </summary>
        public HexFile()
        {

        }

        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void readHexFile(string name)
        {
            byte nBytes = 0;
            int pByte = 9;
            string line;
            string data;

            FileStream input = new FileStream(name, FileMode.Open,FileAccess.Read);

            if(File.Exists("flash.txt"))
                File.Delete("flash.txt");

            FileStream output = new FileStream("flash.txt", FileMode.Create, FileAccess.Write);

            try
            {
                StreamReader file = new StreamReader(input, Encoding.ASCII);
                StreamWriter program = new StreamWriter(output, Encoding.ASCII);

                while (true) 
                {
                    line = file.ReadLine();
                    //line = br.ReadString();
                    if (line.Equals(":00000001FF"))
                        break;

                    nBytes = Convert.ToByte(line.Substring(1, 2),16);
                    size += nBytes;

                    for (int i = 0; i < nBytes / 2; i++)
                    {
                        data = line.Substring(pByte, 2) + "\t" + line.Substring(pByte + 2, 2);
                        program.WriteLine(data);
                        pByte += 4;
                    }
                    line = "";
                    pByte = 9;
                    
                }

                file.Close();
                program.Close();
                input.Close();
                output.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "OPEN FILE", MessageBoxButtons.OK);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int getFileSize()
        {
            return size;
        }

        #endregion
    }
}
