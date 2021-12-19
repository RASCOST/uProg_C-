namespace uPROG2
{
    partial class FormUPROG
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.richTextBoxConsole = new System.Windows.Forms.RichTextBox();
            this.buttonReadFlash = new System.Windows.Forms.Button();
            this.buttonWriteFlash = new System.Windows.Forms.Button();
            this.buttonEraseFlash = new System.Windows.Forms.Button();
            this.textBoxBrowser = new System.Windows.Forms.TextBox();
            this.buttonBrowser = new System.Windows.Forms.Button();
            this.groupBoxProgMode = new System.Windows.Forms.GroupBox();
            this.labelUnit = new System.Windows.Forms.Label();
            this.textBoxFrequency = new System.Windows.Forms.TextBox();
            this.labelFrequency = new System.Windows.Forms.Label();
            this.radioButtonSPI = new System.Windows.Forms.RadioButton();
            this.radioButtonUSB = new System.Windows.Forms.RadioButton();
            this.radioButtonSerial = new System.Windows.Forms.RadioButton();
            this.groupBoxFlash = new System.Windows.Forms.GroupBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBoxEEPROM = new System.Windows.Forms.GroupBox();
            this.buttonEraseEEPROM = new System.Windows.Forms.Button();
            this.buttonWRITEEEPROM = new System.Windows.Forms.Button();
            this.buttonEEPROM = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBoxFuseBits = new System.Windows.Forms.GroupBox();
            this.groupBoxProgMode.SuspendLayout();
            this.groupBoxFlash.SuspendLayout();
            this.groupBoxEEPROM.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBoxConsole
            // 
            this.richTextBoxConsole.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.richTextBoxConsole.DetectUrls = false;
            this.richTextBoxConsole.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxConsole.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.richTextBoxConsole.Location = new System.Drawing.Point(12, 12);
            this.richTextBoxConsole.Name = "richTextBoxConsole";
            this.richTextBoxConsole.ReadOnly = true;
            this.richTextBoxConsole.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBoxConsole.Size = new System.Drawing.Size(252, 152);
            this.richTextBoxConsole.TabIndex = 0;
            this.richTextBoxConsole.Text = "";
            // 
            // buttonReadFlash
            // 
            this.buttonReadFlash.Location = new System.Drawing.Point(6, 26);
            this.buttonReadFlash.Name = "buttonReadFlash";
            this.buttonReadFlash.Size = new System.Drawing.Size(75, 23);
            this.buttonReadFlash.TabIndex = 1;
            this.buttonReadFlash.Text = "READ";
            this.buttonReadFlash.UseVisualStyleBackColor = true;
            this.buttonReadFlash.Click += new System.EventHandler(this.buttonReadFlash_Click);
            // 
            // buttonWriteFlash
            // 
            this.buttonWriteFlash.Location = new System.Drawing.Point(87, 26);
            this.buttonWriteFlash.Name = "buttonWriteFlash";
            this.buttonWriteFlash.Size = new System.Drawing.Size(75, 23);
            this.buttonWriteFlash.TabIndex = 2;
            this.buttonWriteFlash.Text = "WRITE";
            this.buttonWriteFlash.UseVisualStyleBackColor = true;
            this.buttonWriteFlash.Click += new System.EventHandler(this.buttonWriteFlash_Click);
            // 
            // buttonEraseFlash
            // 
            this.buttonEraseFlash.Location = new System.Drawing.Point(168, 26);
            this.buttonEraseFlash.Name = "buttonEraseFlash";
            this.buttonEraseFlash.Size = new System.Drawing.Size(75, 23);
            this.buttonEraseFlash.TabIndex = 3;
            this.buttonEraseFlash.Text = "ERASE";
            this.buttonEraseFlash.UseVisualStyleBackColor = true;
            this.buttonEraseFlash.Click += new System.EventHandler(this.buttonEraseFlash_Click);
            // 
            // textBoxBrowser
            // 
            this.textBoxBrowser.Location = new System.Drawing.Point(12, 312);
            this.textBoxBrowser.Name = "textBoxBrowser";
            this.textBoxBrowser.Size = new System.Drawing.Size(171, 20);
            this.textBoxBrowser.TabIndex = 4;
            // 
            // buttonBrowser
            // 
            this.buttonBrowser.Location = new System.Drawing.Point(189, 312);
            this.buttonBrowser.Name = "buttonBrowser";
            this.buttonBrowser.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowser.TabIndex = 5;
            this.buttonBrowser.Text = "Browse";
            this.buttonBrowser.UseVisualStyleBackColor = true;
            this.buttonBrowser.Click += new System.EventHandler(this.buttonBrowser_Click);
            // 
            // groupBoxProgMode
            // 
            this.groupBoxProgMode.Controls.Add(this.labelUnit);
            this.groupBoxProgMode.Controls.Add(this.textBoxFrequency);
            this.groupBoxProgMode.Controls.Add(this.labelFrequency);
            this.groupBoxProgMode.Controls.Add(this.radioButtonSPI);
            this.groupBoxProgMode.Controls.Add(this.radioButtonUSB);
            this.groupBoxProgMode.Controls.Add(this.radioButtonSerial);
            this.groupBoxProgMode.Location = new System.Drawing.Point(280, 12);
            this.groupBoxProgMode.Name = "groupBoxProgMode";
            this.groupBoxProgMode.Size = new System.Drawing.Size(119, 128);
            this.groupBoxProgMode.TabIndex = 6;
            this.groupBoxProgMode.TabStop = false;
            this.groupBoxProgMode.Text = "Programming Mode";
            // 
            // labelUnit
            // 
            this.labelUnit.AutoSize = true;
            this.labelUnit.Location = new System.Drawing.Point(84, 83);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(29, 13);
            this.labelUnit.TabIndex = 5;
            this.labelUnit.Text = "MHz";
            // 
            // textBoxFrequency
            // 
            this.textBoxFrequency.Enabled = false;
            this.textBoxFrequency.Location = new System.Drawing.Point(9, 80);
            this.textBoxFrequency.Name = "textBoxFrequency";
            this.textBoxFrequency.Size = new System.Drawing.Size(71, 20);
            this.textBoxFrequency.TabIndex = 4;
            this.textBoxFrequency.TextChanged += new System.EventHandler(this.textBoxFrequency_TextChanged);
            // 
            // labelFrequency
            // 
            this.labelFrequency.AutoSize = true;
            this.labelFrequency.Location = new System.Drawing.Point(6, 63);
            this.labelFrequency.Name = "labelFrequency";
            this.labelFrequency.Size = new System.Drawing.Size(88, 13);
            this.labelFrequency.TabIndex = 3;
            this.labelFrequency.Text = "Cristal Frequency";
            // 
            // radioButtonSPI
            // 
            this.radioButtonSPI.AutoSize = true;
            this.radioButtonSPI.Location = new System.Drawing.Point(6, 43);
            this.radioButtonSPI.Name = "radioButtonSPI";
            this.radioButtonSPI.Size = new System.Drawing.Size(67, 17);
            this.radioButtonSPI.TabIndex = 2;
            this.radioButtonSPI.TabStop = true;
            this.radioButtonSPI.Text = "USB SPI";
            this.radioButtonSPI.UseVisualStyleBackColor = true;
            this.radioButtonSPI.CheckedChanged += new System.EventHandler(this.radioButtonSPI_CheckedChanged);
            // 
            // radioButtonUSB
            // 
            this.radioButtonUSB.AutoSize = true;
            this.radioButtonUSB.Location = new System.Drawing.Point(9, 106);
            this.radioButtonUSB.Name = "radioButtonUSB";
            this.radioButtonUSB.Size = new System.Drawing.Size(47, 17);
            this.radioButtonUSB.TabIndex = 1;
            this.radioButtonUSB.TabStop = true;
            this.radioButtonUSB.Text = "USB";
            this.radioButtonUSB.UseVisualStyleBackColor = true;
            this.radioButtonUSB.CheckedChanged += new System.EventHandler(this.radioButtonUSB_CheckedChanged);
            // 
            // radioButtonSerial
            // 
            this.radioButtonSerial.AutoSize = true;
            this.radioButtonSerial.Location = new System.Drawing.Point(7, 19);
            this.radioButtonSerial.Name = "radioButtonSerial";
            this.radioButtonSerial.Size = new System.Drawing.Size(73, 17);
            this.radioButtonSerial.TabIndex = 0;
            this.radioButtonSerial.TabStop = true;
            this.radioButtonSerial.Text = "Serial Port";
            this.radioButtonSerial.UseVisualStyleBackColor = true;
            this.radioButtonSerial.CheckedChanged += new System.EventHandler(this.radioButtonSerial_CheckedChanged);
            // 
            // groupBoxFlash
            // 
            this.groupBoxFlash.Controls.Add(this.buttonReadFlash);
            this.groupBoxFlash.Controls.Add(this.buttonWriteFlash);
            this.groupBoxFlash.Controls.Add(this.buttonEraseFlash);
            this.groupBoxFlash.Location = new System.Drawing.Point(12, 170);
            this.groupBoxFlash.Name = "groupBoxFlash";
            this.groupBoxFlash.Size = new System.Drawing.Size(252, 64);
            this.groupBoxFlash.TabIndex = 7;
            this.groupBoxFlash.TabStop = false;
            this.groupBoxFlash.Text = "Flash";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(13, 345);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(251, 23);
            this.progressBar1.TabIndex = 8;
            // 
            // groupBoxEEPROM
            // 
            this.groupBoxEEPROM.Controls.Add(this.buttonEraseEEPROM);
            this.groupBoxEEPROM.Controls.Add(this.buttonWRITEEEPROM);
            this.groupBoxEEPROM.Controls.Add(this.buttonEEPROM);
            this.groupBoxEEPROM.Location = new System.Drawing.Point(12, 252);
            this.groupBoxEEPROM.Name = "groupBoxEEPROM";
            this.groupBoxEEPROM.Size = new System.Drawing.Size(252, 54);
            this.groupBoxEEPROM.TabIndex = 9;
            this.groupBoxEEPROM.TabStop = false;
            this.groupBoxEEPROM.Text = "EEPROM";
            // 
            // buttonEraseEEPROM
            // 
            this.buttonEraseEEPROM.Location = new System.Drawing.Point(170, 20);
            this.buttonEraseEEPROM.Name = "buttonEraseEEPROM";
            this.buttonEraseEEPROM.Size = new System.Drawing.Size(75, 23);
            this.buttonEraseEEPROM.TabIndex = 2;
            this.buttonEraseEEPROM.Text = "ERASE";
            this.buttonEraseEEPROM.UseVisualStyleBackColor = true;
            // 
            // buttonWRITEEEPROM
            // 
            this.buttonWRITEEEPROM.Location = new System.Drawing.Point(88, 20);
            this.buttonWRITEEEPROM.Name = "buttonWRITEEEPROM";
            this.buttonWRITEEEPROM.Size = new System.Drawing.Size(75, 23);
            this.buttonWRITEEEPROM.TabIndex = 1;
            this.buttonWRITEEEPROM.Text = "WRITE";
            this.buttonWRITEEEPROM.UseVisualStyleBackColor = true;
            // 
            // buttonEEPROM
            // 
            this.buttonEEPROM.Location = new System.Drawing.Point(6, 20);
            this.buttonEEPROM.Name = "buttonEEPROM";
            this.buttonEEPROM.Size = new System.Drawing.Size(75, 23);
            this.buttonEEPROM.TabIndex = 0;
            this.buttonEEPROM.Text = "READ";
            this.buttonEEPROM.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(280, 196);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(123, 74);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Lock Bits";
            // 
            // groupBoxFuseBits
            // 
            this.groupBoxFuseBits.Location = new System.Drawing.Point(280, 272);
            this.groupBoxFuseBits.Name = "groupBoxFuseBits";
            this.groupBoxFuseBits.Size = new System.Drawing.Size(123, 96);
            this.groupBoxFuseBits.TabIndex = 11;
            this.groupBoxFuseBits.TabStop = false;
            this.groupBoxFuseBits.Text = "Fuse Bits";
            this.groupBoxFuseBits.Leave += new System.EventHandler(this.FormUPROG_Load);
            // 
            // FormUPROG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 380);
            this.Controls.Add(this.groupBoxFuseBits);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxEEPROM);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.groupBoxFlash);
            this.Controls.Add(this.groupBoxProgMode);
            this.Controls.Add(this.buttonBrowser);
            this.Controls.Add(this.textBoxBrowser);
            this.Controls.Add(this.richTextBoxConsole);
            this.Name = "FormUPROG";
            this.Text = "uPROG";
            this.Load += new System.EventHandler(this.FormUPROG_Load);
            this.Leave += new System.EventHandler(this.FormUPROG_Leave);
            this.groupBoxProgMode.ResumeLayout(false);
            this.groupBoxProgMode.PerformLayout();
            this.groupBoxFlash.ResumeLayout(false);
            this.groupBoxEEPROM.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxConsole;
        private System.Windows.Forms.Button buttonReadFlash;
        private System.Windows.Forms.Button buttonWriteFlash;
        private System.Windows.Forms.Button buttonEraseFlash;
        private System.Windows.Forms.TextBox textBoxBrowser;
        private System.Windows.Forms.Button buttonBrowser;
        private System.Windows.Forms.GroupBox groupBoxProgMode;
        private System.Windows.Forms.RadioButton radioButtonUSB;
        private System.Windows.Forms.RadioButton radioButtonSerial;
        private System.Windows.Forms.GroupBox groupBoxFlash;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.GroupBox groupBoxEEPROM;
        private System.Windows.Forms.Button buttonEraseEEPROM;
        private System.Windows.Forms.Button buttonWRITEEEPROM;
        private System.Windows.Forms.Button buttonEEPROM;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBoxFuseBits;
        private System.Windows.Forms.RadioButton radioButtonSPI;
        private System.Windows.Forms.Label labelFrequency;
        private System.Windows.Forms.TextBox textBoxFrequency;
        private System.Windows.Forms.Label labelUnit;
    }
}

