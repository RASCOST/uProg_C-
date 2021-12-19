using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uPROG2
{
    public struct SupportedDevices
    {
        public string name;
        public int flash;
        public int flashPageSize;
        public int flashNumPages;
        public int eeprom;
        public int eepromPageSize;
        public int eepromNumPages;
        public int signatureByte1;
        public int signatureByte2;
        public int signatureByte3;
        public int sigByte1Ad;
        public int sigByte2Ad;
        public int sigByte3Ad;
        public bool pollingCmd;
    }
}
