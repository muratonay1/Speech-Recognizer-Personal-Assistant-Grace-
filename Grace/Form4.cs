using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Collections;


namespace Grace
{
    public partial class Form4 : Form
    {
        private int tick;
        public Form4()
        {
            InitializeComponent();
        }
        Form1 frm1 = new Form1();
        private void Form4_Load(object sender, EventArgs e)
        {
            string line;
            StreamReader sr = new StreamReader(@"MusicUrl.txt",true);
            line = sr.ReadLine();
            sr.Close();
            if (line=="" || line==null)
            {
                MessageBox.Show("URL girilecek");
                button1.Visible = true;
                textBox1.Visible = true;
                label1.Visible = true;
            }
            else
            {                
                textBox1.Visible = false;
                label1.Visible = false;
                label2.Visible = true;
                this.BackColor = Color.Black;
                label2.BackColor = Color.Black;
                label2.ForeColor = Color.White;
                button1.BackColor = Color.Black;
                button1.ForeColor = Color.Black;
                MessageBox.Show("Grace'e Yönlendirileceksin");
                textBox1.Text = line;
                timer1.Enabled = true;
                timer1.Start();
                frm1.yol(line);
            }           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            tick++;
            if(tick==25)
            {
                this.button1.PerformClick();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StreamWriter Yaz = new StreamWriter(@"MusicUrl.txt", true);
            Yaz.WriteLine(textBox1.Text);
            Yaz.Close();            
            frm1.yol(this.textBox1.Text);
            this.Hide();
            frm1.Show();
        }
    }
}
