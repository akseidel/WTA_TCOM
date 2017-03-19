namespace PlunkOMaticTCOM {
    partial class FormMsg {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.labelMsg = new System.Windows.Forms.Label();
            this.labelESC = new System.Windows.Forms.Label();
            this.labelPurpose = new System.Windows.Forms.Label();
            this.panelPurpose = new System.Windows.Forms.Panel();
            this.panelMsg = new System.Windows.Forms.Panel();
            this.panelEsc = new System.Windows.Forms.Panel();
            this.panelPurpose.SuspendLayout();
            this.panelMsg.SuspendLayout();
            this.panelEsc.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelMsg
            // 
            this.labelMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMsg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMsg.Location = new System.Drawing.Point(0, 0);
            this.labelMsg.Name = "labelMsg";
            this.labelMsg.Size = new System.Drawing.Size(260, 66);
            this.labelMsg.TabIndex = 0;
            this.labelMsg.Text = "Prompt Message Prompt Message\r\nPrompt Message Prompt\r\nPrompt Message Prompt\r\nProm" +
    "pt Message Prompt";
            this.labelMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelMsg.TextChanged += new System.EventHandler(this.labelMsg_TextChanged);
            this.labelMsg.MouseClick += new System.Windows.Forms.MouseEventHandler(this.labelMsg_MouseClick);
            // 
            // labelESC
            // 
            this.labelESC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelESC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelESC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelESC.Location = new System.Drawing.Point(0, 0);
            this.labelESC.Margin = new System.Windows.Forms.Padding(0);
            this.labelESC.Name = "labelESC";
            this.labelESC.Padding = new System.Windows.Forms.Padding(2);
            this.labelESC.Size = new System.Drawing.Size(260, 24);
            this.labelESC.TabIndex = 1;
            this.labelESC.Text = "(ESC cancels)";
            this.labelESC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelPurpose
            // 
            this.labelPurpose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPurpose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelPurpose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPurpose.Location = new System.Drawing.Point(0, 0);
            this.labelPurpose.Name = "labelPurpose";
            this.labelPurpose.Size = new System.Drawing.Size(260, 30);
            this.labelPurpose.TabIndex = 2;
            this.labelPurpose.Text = "Purpose";
            this.labelPurpose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelPurpose
            // 
            this.panelPurpose.Controls.Add(this.labelPurpose);
            this.panelPurpose.Location = new System.Drawing.Point(0, 0);
            this.panelPurpose.Name = "panelPurpose";
            this.panelPurpose.Size = new System.Drawing.Size(260, 30);
            this.panelPurpose.TabIndex = 3;
            // 
            // panelMsg
            // 
            this.panelMsg.Controls.Add(this.labelMsg);
            this.panelMsg.Location = new System.Drawing.Point(0, 30);
            this.panelMsg.Name = "panelMsg";
            this.panelMsg.Size = new System.Drawing.Size(260, 66);
            this.panelMsg.TabIndex = 4;
            // 
            // panelEsc
            // 
            this.panelEsc.Controls.Add(this.labelESC);
            this.panelEsc.Location = new System.Drawing.Point(0, 96);
            this.panelEsc.Margin = new System.Windows.Forms.Padding(0);
            this.panelEsc.Name = "panelEsc";
            this.panelEsc.Size = new System.Drawing.Size(260, 24);
            this.panelEsc.TabIndex = 5;
            // 
            // FormMsg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(260, 120);
            this.ControlBox = false;
            this.Controls.Add(this.panelEsc);
            this.Controls.Add(this.panelMsg);
            this.Controls.Add(this.panelPurpose);
            this.DataBindings.Add(new System.Windows.Forms.Binding("Location", global::PlunkOMaticTCOM.Properties.Settings.Default, "MyLoc", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = global::PlunkOMaticTCOM.Properties.Settings.Default.MyLoc;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMsg";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Move As Needed";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMsg_FormClosing);
            this.Load += new System.EventHandler(this.FormMsg_Load);
            this.LocationChanged += new System.EventHandler(this.FormMsg_LocationChanged);
            this.panelPurpose.ResumeLayout(false);
            this.panelMsg.ResumeLayout(false);
            this.panelEsc.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelMsg;
        private System.Windows.Forms.Label labelESC;
        private System.Windows.Forms.Label labelPurpose;
        private System.Windows.Forms.Panel panelPurpose;
        private System.Windows.Forms.Panel panelMsg;
        private System.Windows.Forms.Panel panelEsc;
    }
}