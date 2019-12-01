using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grace
{
    public partial class Form2 : Form
    {
        private int tick;
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)//simon wake up gifi bekleme süresi
        {
            tick++;
            if(tick==38)
            {
                timer1.Stop();
                tick = 0;
                timer1.Enabled = false;
                this.Close();
            }
        }
    }
}
