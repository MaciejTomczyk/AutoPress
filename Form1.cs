using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace AutoPress
{
    public partial class Form1 : Form
    {
        int interval = 0;
        byte keyGlobal = 0;
    	int oldInterval = 0;
    	int oldInterval2 = 0;
    	int pressTimes = 0;

    	Random r = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        [DllImport("User32.dll", SetLastError = true)]

        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        //const int VK_UP = 0x26; //up key
        //const int VK_DOWN = 0x28;  //down key
        //const int VK_LEFT = 0x25;
        //const int VK_RIGHT = 0x27;
        //const int VK_LWIN = 0x5B;
        //const int H_KEY = 0x48;

        const int VK_OEM_3 = 0x55;
		
        const uint KEYEVENTF_KEYUP = 0x0002;
        const uint KEYEVENTF_EXTENDEDKEY = 0x0001;

        void press(int theKey)
        {
            keybd_event((byte)theKey, 0x45, KEYEVENTF_EXTENDEDKEY | 0, 0 );
            Thread.Sleep(r.Next(1,15));
            keybd_event((byte)theKey, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }
        void press(byte theKey)
        {
            keybd_event(theKey, 0x45, KEYEVENTF_EXTENDEDKEY | 0, 0 );
            Thread.Sleep(r.Next(1,15));
            keybd_event(theKey, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if(timer1.Enabled == true)
            {
        	timer1.Enabled = false;
            richTextBox1.Text = richTextBox1.Text + "Stopped! ";
            textBox2.Enabled = true;
            this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length;
            this.richTextBox1.ScrollToCaret();
            textBox1.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
        	if(timer1.Enabled == false)
        	{
        	    pressTimes = 0;
                this.Text = ("Ticks: " + pressTimes);
                timer1.Enabled = true;
                richTextBox1.Text = richTextBox1.Text + "Started! ";
                textBox2.Enabled = false;
                int x = 0;
            
            // Interval check
                    if (textBox2.Text != "" && int.TryParse(textBox2.Text, out x))
                    {
                        timer1.Interval = Math.Abs(int.Parse(textBox2.Text));
                        textBox2.Text = Math.Abs(int.Parse(textBox2.Text)).ToString();	
                        oldInterval = Math.Abs(int.Parse(textBox2.Text));
                        oldInterval2 = Math.Abs(int.Parse(textBox2.Text)) + 1600;
                    }
            else
            {
                MessageBox.Show("Invalid value was entered, selecting 1000");
                timer1.Interval = 1000;
                textBox2.Text = "1000";
                oldInterval = 1000;
                oldInterval2 = 1000 + 1600;
            }
            
            // Key check
            if (textBox1.Text == "" | textBox1.Text == ("Using default(U)"))
            {
                textBox1.Text = ("Using default(U)");
                textBox1.Enabled = false;
            }
            else
            {
            	try
            	{
            		keyGlobal = Convert.ToByte(textBox1.Text, 16);
            		textBox1.Enabled = false;
            	}
            	catch(Exception ex)
            	{
            		button1.PerformClick();
            		textBox1.Text = "";
            		MessageBox.Show(ex.Message);
            	}
            		
            }
            this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length;
            this.richTextBox1.ScrollToCaret();
        }
       }
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            richTextBox1.Text = "Console: ";
            textBox2.Text = "1000";
            linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            linkLabel1.Text = "Key in byte value(see link)";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pressTimes++;
            this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length;
            this.richTextBox1.ScrollToCaret();
            this.Text = ("Ticks: " + pressTimes);			
			interval = r.Next(oldInterval, oldInterval2);
			richTextBox1.Text = richTextBox1.Text + timer1.Interval + " ";
			timer1.Interval = interval;
            if (textBox1.Text == "" | textBox1.Text == "Using default(U)")
            {
            	press(VK_OEM_3);
            }
            else
            {
            	press(keyGlobal);
            }
        }
        private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
    	{
            this.linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start("http://msdn.microsoft.com/en-us/library/dd375731%28v=vs.85%29.aspx");
    	}
        
        void RichTextBox1TextChanged(object sender, EventArgs e)
        {
        	this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length;
            this.richTextBox1.ScrollToCaret();
        }
    }
}