using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlunkOMaticTCOM {
    public partial class FormMsg : Form {
        private System.Drawing.Color ClrA;
        private System.Drawing.Color ClrB;
        private bool windJustMoved = false;
        private int orgHT = 120;
        private int orgWT = 260;
        private System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        

        public FormMsg() {
            InitializeComponent();
        }

        public void SetMsg(string _msg,   string  _purpose = ""){
            labelMsg.Text = _msg;
            labelPurpose.Text = _purpose;
           }

        private void FormMsg_Load(object sender, EventArgs e) {
            this.Location = Properties.Settings.Default.MyLoc;
            RandomColorPair();
            this.Height = orgHT;
            this.Width = orgWT;
           }

        private void FormMsg_FormClosing(object sender, FormClosingEventArgs e) {
            Properties.Settings.Default.MyLoc = this.Location;
            Properties.Settings.Default.Save();
        }

        private void FlipColor() {
            if (this.BackColor == ClrA) {
                this.BackColor = ClrB;
            } else {
                this.BackColor = ClrA;
            }
        }

        private void RandomColorPair() {
            Random rand = new Random();
            int randInt = rand.Next(0, 4);
            switch (randInt) {
                case 0:
                    ClrA = System.Drawing.Color.Aqua;
                    ClrB = System.Drawing.Color.Aquamarine;
                    break;
                case 1:
                    ClrA = System.Drawing.Color.PapayaWhip;
                    ClrB = System.Drawing.Color.PeachPuff;
                    break;
                case 2:
                    ClrA = System.Drawing.Color.Bisque;
                    ClrB = System.Drawing.Color.BlanchedAlmond;
                    break;
                case 3:
                    ClrA = System.Drawing.Color.Lavender;
                    ClrB = System.Drawing.Color.LavenderBlush;
                    break;
                default:
                    ClrA = System.Drawing.Color.Aqua;
                    ClrB = System.Drawing.Color.Aquamarine;
                    break;
            }
        }

        private void labelMsg_TextChanged(object sender, EventArgs e) {
            FlipColor();
        }
  
        private void FormMsg_LocationChanged(object sender, EventArgs e) {
            HideControls();
        }

        private void SetBorderTimer() {
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e) {
            dispatcherTimer.Stop();
            HideControls();
        }

        private void splitContainerOuter_MouseEnter(object sender, EventArgs e) {
            ShowControls();
        }

        private void labelMsg_MouseClick(object sender, MouseEventArgs e) {
            ShowControls();
        }

        private void HideControls() {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Height = orgHT;
            this.Width = orgWT;
        }       

        private void ShowControls() {
            if (!windJustMoved) {
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
                SetBorderTimer();
            } else {
                windJustMoved = false;
            }
        }

       

    }
}
