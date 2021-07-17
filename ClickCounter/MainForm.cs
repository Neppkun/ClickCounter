namespace ClickCounter
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    internal class MainForm : Form
    {
        private IContainer components;
        private System.Windows.Forms.Timer timer1;
        private Label lbClicksVal;
        private Label lbLabel;
        private Label lbMaxClickRate;
        private LinkLabel llbWebsite;
        private int clicks;
        private ulong totalClicks;
        private int maxClickRate;
        private bool running;
        private int tmpN;
        private string titleFormat;

        public MainForm()
        {
            this.InitializeComponent();
            base.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            this.clicks = 0;
            this.maxClickRate = 0;
            this.totalClicks = 0L;
            this.running = true;
            new Thread(new ThreadStart(this.runClickCounter)).Start();
            CultureInfo cultureInfo = CultureInfo.GetCultureInfo("de");
            if (CultureInfo.CurrentCulture.Equals(cultureInfo) || CultureInfo.CurrentCulture.Parent.Equals(cultureInfo))
            {
                this.lbLabel.Text = "Klicks pro Sekunde";
                this.titleFormat = "{0} Klicks pro Sekunde";
            }
            else
            {
                this.lbLabel.Text = "Clicks per second";
                this.titleFormat = "{0} clicks per second";
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.running = false;
        }

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);
        private void InitializeComponent()
        {
            this.components = new Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lbClicksVal = new Label();
            this.lbLabel = new Label();
            this.lbMaxClickRate = new Label();
            this.llbWebsite = new LinkLabel();
            base.SuspendLayout();
            this.timer1.Enabled = true;
            this.timer1.Interval = 0x1f4;
            this.timer1.Tick += new EventHandler(this.timer1_Tick);
            this.lbClicksVal.Dock = DockStyle.Fill;
            this.lbClicksVal.Font = new Font("Microsoft Sans Serif", 48f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lbClicksVal.Location = new Point(0, 0);
            this.lbClicksVal.Name = "lbClicksVal";
            this.lbClicksVal.Size = new Size(0xf4, 0x43);
            this.lbClicksVal.TabIndex = 0;
            this.lbClicksVal.Text = "0";
            this.lbClicksVal.TextAlign = ContentAlignment.MiddleCenter;
            this.lbLabel.Dock = DockStyle.Bottom;
            this.lbLabel.Location = new Point(0, 0x43);
            this.lbLabel.Name = "lbLabel";
            this.lbLabel.Size = new Size(0xf4, 0x12);
            this.lbLabel.TabIndex = 1;
            this.lbLabel.TextAlign = ContentAlignment.TopCenter;
            this.lbMaxClickRate.Dock = DockStyle.Bottom;
            this.lbMaxClickRate.Location = new Point(0, 0x55);
            this.lbMaxClickRate.Name = "lbMaxClickRate";
            this.lbMaxClickRate.Size = new Size(0xf4, 13);
            this.lbMaxClickRate.TabIndex = 2;
            this.llbWebsite.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.llbWebsite.Location = new Point(0x9d, 0x55);
            this.llbWebsite.Name = "llbWebsite";
            this.llbWebsite.Size = new Size(0x57, 13);
            this.llbWebsite.TabIndex = 3;
            this.llbWebsite.TabStop = true;
            this.llbWebsite.Text = "https://fabi.me";
            this.llbWebsite.TextAlign = ContentAlignment.BottomRight;
            this.llbWebsite.LinkClicked += new LinkLabelLinkClickedEventHandler(this.llbWebsite_LinkClicked);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0xf4, 0x62);
            base.Controls.Add(this.llbWebsite);
            base.Controls.Add(this.lbClicksVal);
            base.Controls.Add(this.lbLabel);
            base.Controls.Add(this.lbMaxClickRate);
            base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "MainForm";
            base.FormClosed += new FormClosedEventHandler(this.Form1_FormClosed);
            base.ResumeLayout(false);
        }

        private void llbWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(this.llbWebsite.Text);
        }

        private static bool MouseIsPressed() => 
            (GetAsyncKeyState(Keys.LButton) != 0);

        private void runClickCounter()
        {
            bool flag = false;
            int num = 0;
            while (this.running)
            {
                bool flag2 = MouseIsPressed();
                if (flag2 && !flag)
                {
                    this.clicks++;
                }
                flag = flag2;
                if (num++ >= this.tmpN)
                {
                    Thread.Sleep(0);
                    num = 0;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            totalClicks += (ulong)clicks;
            this.tmpN = 0x1388 + (clicks * 20);
            if (this.clicks > this.maxClickRate)
            {
                this.maxClickRate = this.clicks;
            }
            this.Text = string.Format(titleFormat, clicks);
            this.lbClicksVal.Text = this.clicks.ToString();
            this.lbMaxClickRate.Text = $"Max.: {this.maxClickRate}  Total: {this.totalClicks}";
            this.clicks = 0;
        }
    }
}

