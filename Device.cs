using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uPROG2
{
    public struct Device
    {
        string name = string.Empty;
        int flash = 0;
        int pageSize = 0;
        int numPages = 0;
        int[] signature = new int[3];
        int[] calibration = new int[4];
    }
}
