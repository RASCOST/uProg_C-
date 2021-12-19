using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uPROG2
{
    class WriteConsoleEvent: EventArgs
    {
        private string newDataConsole;

        public string NewDataConsole
        {
            set { newDataConsole = value; }

            get{ return newDataConsole; }
        }
    }
}
