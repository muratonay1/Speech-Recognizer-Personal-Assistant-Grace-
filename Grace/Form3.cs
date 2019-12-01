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
    public partial class Form3 : Form
    {
        
        private int tmr1;
       
        public Form3()
        {
            InitializeComponent();
        }        
        private void Form3_Load(object sender, EventArgs e)
        {
            label8.Text = "";//eklendi/eklenemedi yazısı butonun altında
            axWindowsMediaPlayer1.URL = "Costumer Service/costumer_song.mp3";
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }
        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {
            
        }
        private void button1_Click(object sender, EventArgs e) // PHONE LİST KAYIT EKLEME (SONA EKLER)
        {
            if(textBox1.Text != "" && textBox2.Text != "")
            {
                timer1.Enabled = true;
                timer1.Start();
                StreamWriter Yaz = new StreamWriter(@"PhoneList.txt", true);
                Yaz.WriteLine(textBox1.Text + " " + textBox2.Text);
                Yaz.Close();
                textBox1.Clear();
                textBox2.Clear();
                listBox1.Items.Clear();
                //Dosya okuma buradan başlıyor
                string dosya_yolu = @"PhoneList.txt";
                FileStream fs = new FileStream(dosya_yolu, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                string yazi = sr.ReadLine();
                while (yazi != null)
                {
                    listBox1.Items.Add(yazi);
                    yazi = sr.ReadLine();
                }
                sr.Close();
                fs.Close();
                //dosya okuma burada bitiyor
                label8.ForeColor = Color.LightGreen;//telefon numarası ekleme sırasında hata olursa uyarı veren label kontrolü
                label8.Text = "Eklendi";
            }
            else if(textBox1.Text == "" || textBox2.Text == "")
            {
                timer1.Enabled = true;
                timer1.Start();
                label8.ForeColor = Color.Brown;
                label8.Text = "Eklenemedi";
            }
            
        }
        private void timer1_Tick(object sender, EventArgs e)//eklendi eklenmedi yazısı gözükme kaybolma süresi
        {
            tmr1++;
            if(tmr1 == 30)
            {
                label8.Text = "";
                timer1.Enabled = false;
                timer1.Stop();
                tmr1 = 0;
            }
        }

        
    }
}
