namespace PlunkOMaticTCOM {
    partial class PlunkOMaticTCOM {
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
            this.components = new System.ComponentModel.Container();
            this.toolTipReporter = new System.Windows.Forms.ToolTip(this.components);
            this.buttonPlunk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonPlunk
            // 
            this.buttonPlunk.Location = new System.Drawing.Point(227, 26);
            this.buttonPlunk.Name = "buttonPlunk";
            this.buttonPlunk.Size = new System.Drawing.Size(75, 23);
            this.buttonPlunk.TabIndex = 0;
            this.buttonPlunk.Text = "Plunk";
            this.buttonPlunk.UseVisualStyleBackColor = true;
            this.buttonPlunk.MouseClick += new System.Windows.Forms.MouseEventHandler(this.buttonPlunk_MouseClick);
            // 
            // PlunkOMatic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(426, 207);
            this.Controls.Add(this.buttonPlunk);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PlunkOMatic";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Plunk-O-Matic";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTipReporter;
        private System.Windows.Forms.Button buttonPlunk;
    }
}