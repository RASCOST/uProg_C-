using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace uPROG2
{
    public partial class FormSelDev : Form
    {
        private const int CP_NOCLOSE_BUTTON = 0x200;

        private FormUPROG parent;
        private string device;

        public FormSelDev(FormUPROG form1)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            parent = form1;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        } 

        private void FormSelDev_Load(object sender, EventArgs e)
        {
            buttonSelect.Enabled = false;

            treeViewDevice.Nodes.Add("Microcontroller");
            //treeViewDevice.Nodes.Add("EEPROM");

            treeViewDevice.Nodes[0].Nodes.Add("Atmel");

            treeViewDevice.Nodes[0].Nodes[0].Nodes.Add("ATtiny45");
            treeViewDevice.Nodes[0].Nodes[0].Nodes.Add("Atmega8");
            treeViewDevice.Nodes[0].Nodes[0].Nodes.Add("Atmega88");
            treeViewDevice.Nodes[0].Nodes[0].Nodes.Add("Atmega32");

            treeViewDevice.Nodes[0].Nodes.Add("Microchip");
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            parent.getDevice(device);
            this.Close();
        }

        private void treeViewDevice_Click(object sender, EventArgs e)
        {
            if (!buttonSelect.Enabled)
                buttonSelect.Enabled = true;
        }

        private void treeViewDevice_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = treeViewDevice.SelectedNode;

            device = node.Text;
        }      
    }
}
