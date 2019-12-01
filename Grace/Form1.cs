using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition; // speech referans
using System.Speech.Synthesis; //speech referans
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace Grace
{
    public partial class Form1 : Form
    {       
        [DllImport("user32")] // security mode için
        public static extern void LockWorkStation();// security mode için
        private int tmr2;
        public int muzik_sayac=0;
        public int müzik=0,müzik2=0;
        SpeechRecognitionEngine _recognizer = new SpeechRecognitionEngine();
        SpeechRecognitionEngine startlistening = new SpeechRecognitionEngine();
        SpeechSynthesizer simon = new SpeechSynthesizer();        
        Random rnd = new Random();
        Form3 frm3 = new Form3();
        int RecTimeout = 0;
        string yolurl;
        public void yol(string a)//URL PENCERESİNDEN müzik URL Yİ ALAN FONKSİYON
        {
            yolurl = a;
        }
        
        public Form1()
        {
            InitializeComponent();
        }
        public void music_read() // MÜZİĞİ DOSYADAN OKUYAN FONKSİYON
        {
            var url = @"" + yolurl;
            DirectoryInfo music = new DirectoryInfo(url);//@"C:\Users\murat\Desktop\simon\simon\bin\Debug\music" "ÖRNEK OLARAK YAZILMIŞTIR"
            foreach (FileInfo list in music.GetFiles())
            {                
                listBox3.Items.Add(list.Name);
                müzik++;
                müzik2++;
            }
        }
        public void yazi(string gelen_cümle,string gelen_speech)//SÖYLENEN CÜMLEYİ VE CEVABI ALAN FONKSİYON
        {
            listBox1.Items.Add(DateTime.Now.ToString("hh:mm:ss")+"-> "+ gelen_speech);
            listBox2.Items.Add(DateTime.Now.ToString("hh:mm:ss") + "-> " + gelen_cümle);
            simon.SpeakAsync(gelen_cümle);
        }
        private void Form1_Load(object sender, EventArgs e) //FORM LOAD OLAYI
        {
            //asistan ses
            simon.SelectVoiceByHints(VoiceGender.Female);
            //mikr
            _recognizer.SetInputToDefaultAudioDevice();
            //sesli olarak verilecek komutların txt dosyasından okunması
            _recognizer.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines(@"DefaultCommands.txt")))));
            _recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Default_SpeechRecognized);
            _recognizer.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(_recognizer_SpeechRecognized);
            _recognizer.RecognizeAsync(RecognizeMode.Multiple);


            //konuşmayı başlatmak(başlangıçta ve sonraki olaylar için) için oluşturduğumuz seçim listesinden arama yapmak için
            startlistening.SetInputToDefaultAudioDevice();
            startlistening.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines(@"DefaultCommands.txt")))));
            startlistening.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(startlistening_SpeechRecognized);
            _recognizer.RecognizeAsyncCancel();
            startlistening.RecognizeAsync(RecognizeMode.Multiple);
           
        }
        private void Default_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            int ranNum;
            string speech = e.Result.Text;
            string cümle;

            if (speech == "Hello") //------------------------------------------------------
            {
                cümle = "i'm here! Sir!";
                yazi(cümle, speech);
            }           
            if (speech == "Stop Speaking")//------------------------------------------------------
            {
                
                cümle = "if you need me just say wake up!";
                yazi(cümle, speech);
                _recognizer.RecognizeAsyncCancel();
                startlistening.RecognizeAsync(RecognizeMode.Multiple);                
            }
            if (speech == "Opacity")//------------------------------------------------------
            {
                if (this.Opacity == 1.0)
                {
                    cümle = "Visibility Down!";
                    yazi(cümle, speech);
                    this.Opacity = 0.6;

                }
                else if (this.Opacity == 0.6)
                {
                    cümle = "Visibility Up!";
                    yazi(cümle, speech);
                    this.Opacity = 1.0;
                }
            }
            if(speech=="Music")//------------------------------------------------------
            {
                if(groupBox1.Visible==false)
                {
                    listBox3.Items.Clear();
                    cümle = "Music Active!";
                    yazi(cümle, speech);
                    groupBox1.Visible = true;                   
                    music_read();
                }
                else if(groupBox1.Visible==true)
                {
                    cümle = "Music Deactive!";
                    yazi(cümle, speech);
                    groupBox1.Visible = false;
                    label3.Text = "";
                }               
            }
            if(speech=="Play")//------------------------------------------------------
            {
                if(groupBox1.Visible == false)
                {
                    cümle = "Please Say Music!";
                    yazi(cümle, speech);
                }
                else if(groupBox1.Visible==true)
                {
                    if(muzik_sayac != 0)
                    {
                        cümle = "Playing!";
                        yazi(cümle, speech);
                        string sarki = listBox3.Items[muzik_sayac].ToString();
                        listBox3.SetSelected(muzik_sayac, true);
                        muzik_sayac = 0;
                        axWindowsMediaPlayer1.URL = "music/" + sarki;
                        axWindowsMediaPlayer1.Ctlcontrols.play();
                        label3.Text = listBox3.Items[muzik_sayac].ToString();
                    }
                    else
                    {
                        cümle = "Playing!";
                        yazi(cümle, speech);
                        string sarki = listBox3.Items[0].ToString();
                        listBox3.SetSelected(muzik_sayac, true);
                        muzik_sayac = 0;
                        axWindowsMediaPlayer1.URL = "music/" + sarki;
                        axWindowsMediaPlayer1.Ctlcontrols.play();
                        label3.Text = listBox3.Items[muzik_sayac].ToString();
                    }                    
                }
            }
            if(speech=="Next")//------------------------------------------------------
            {
                if(groupBox1.Visible == true)
                {
                    muzik_sayac++;
                    if(muzik_sayac==müzik)
                    {
                        muzik_sayac=0;                       
                    }
                    string sarki = listBox3.Items[muzik_sayac].ToString();
                    listBox3.SetSelected(muzik_sayac, true);            
                    axWindowsMediaPlayer1.URL = "music/" + sarki;
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                    label3.Text = listBox3.Items[muzik_sayac].ToString();
                }
                else if(groupBox1.Visible==false)
                {
                    cümle = "Please Say Music!";
                    yazi(cümle, speech);
                }
            }
            if(speech == "Back")//------------------------------------------------------
            {
                if (groupBox1.Visible == true)
                {
                    muzik_sayac--;
                    if (muzik_sayac== -1)
                    {
                        muzik_sayac = müzik2-1;
                        string sarki = listBox3.Items[muzik_sayac].ToString();
                        listBox3.SetSelected(muzik_sayac, true);                        
                        axWindowsMediaPlayer1.URL = "music/" + sarki;
                        axWindowsMediaPlayer1.Ctlcontrols.play();
                        label3.Text= listBox3.Items[muzik_sayac].ToString();
                    }
                    else
                    {
                        string sarki = listBox3.Items[muzik_sayac].ToString();
                        listBox3.SetSelected(muzik_sayac, true);
                        axWindowsMediaPlayer1.URL = "music/" + sarki;
                        axWindowsMediaPlayer1.Ctlcontrols.play();
                        label3.Text = listBox3.Items[muzik_sayac].ToString();
                    }   
                }
                else if (groupBox1.Visible == false)
                {
                    cümle = "Please Say Music!";
                    yazi(cümle, speech);
                }
            }
            if(speech=="Stop")//-------------------------------------------------------
            {
                if(groupBox1.Visible==true)
                {
                    cümle = "Music is Stopped!";
                    yazi(cümle, speech);
                    axWindowsMediaPlayer1.Ctlcontrols.pause();
                }
                else if(groupBox1.Visible == false)
                {
                    cümle = "Please say music!";
                    yazi(cümle, speech);
                }
            }
            if(speech=="Minimize")//-------------------------------------------------------
            {
                cümle = "Minimize Mod!";
                yazi(cümle, speech);
                this.WindowState = FormWindowState.Minimized;
            }
            if(speech=="Normal")//-------------------------------------------------------
            {
                cümle = "Normal Mod!";
                yazi(cümle, speech);
                this.WindowState = FormWindowState.Normal;
            }
            if(speech == "Agent Mode")//-------------------------------------------------------
            {
                cümle = "Mode Active";
                yazi(cümle, speech);
                this.Opacity = 0.0;
            }
            if (speech == "Where are you")//-------------------------------------------------------
            {
                if(this.Opacity==0.0)
                {
                    cümle = "i'm! Everywhere";
                    yazi(cümle, speech);
                }
                else
                {
                    cümle = "i'm here sir!";
                    yazi(cümle, speech);
                    this.WindowState = FormWindowState.Maximized;                   
                }                
            }
            if(speech=="show me")//-------------------------------------------------------
            {
                cümle = "ok! i'm showing sir";
                yazi(cümle, speech);
                this.Opacity = 1;
            }
            if (speech == "How are you")//-------------------------------------------------------
            {
                cümle = "Thank you sir!, i'm fine. Are you okay?";
                yazi(cümle, speech);
                this.Opacity = 1;
            }
            if (speech=="Open Commands")//-------------------------------------------------------
            {                
                string[] command = (File.ReadAllLines(@"DefaultCommands.txt"));
                listBox4.Items.Clear();
                listBox4.SelectionMode = SelectionMode.None;
                listBox4.Visible = true;
                foreach (string komut in command)
                {
                    listBox4.Items.Add(komut);
                }
            }
            if (speech == "Close Commands")//-------------------------------------------------------
            {               
                listBox4.Visible = false;
            }            
            if(speech == "Youtube")//-------------------------------------------------------
            {
               if(webBrowser1.Visible==false)
                {
                    cümle = "Youtube Active!";
                    yazi(cümle, speech);
                    webBrowser1.Visible = true;
                    webBrowser1.Navigate("http://www.youtube.com");
                }
               else if(webBrowser1.Visible==true)
                {
                    //Pencereyi kapattığımızda müzik halen çalmaya devama ediyor. Bu nedenle önce navigasyon yaptırıp sonra kapatıyoruz.
                    webBrowser1.Navigate("http://www.google.com");
                    cümle = "Youtube Deactive!";
                    yazi(cümle, speech);
                    simon.SpeakAsync(cümle);
                    
                    webBrowser1.Visible = false;
                }
            }
            if(speech == "Costumer Service")//----------------------------------------------------
            {
                cümle = "Well come to costumer service";
                yazi(cümle, speech);
                frm3.axWindowsMediaPlayer1.Ctlcontrols.play();
                frm3.Show();

            }
            if(speech=="Thank you")//---------------------------------------------------------------
            {
                cümle = "No Problem sir!";
                yazi(cümle, speech);
            }
            if(speech=="close costumer service")
            {
                cümle = "Service is Closing!";
                yazi(cümle, speech);
                frm3.axWindowsMediaPlayer1.Ctlcontrols.stop();
                frm3.Hide();
               
            }
            if(speech=="Clear")//---------------------------------------------------------------
            {
                cümle = "All Messages is clean!";
                yazi(cümle, speech);
                listBox1.Items.Clear();
                listBox2.Items.Clear();
            }
            if(speech=="Phone List")//---------------------------------------------------------------
            {                
                if(frm3.Visible==true)
                {
                    if(frm3.groupBox1.Visible==true)
                    {
                        cümle = "Phone List  Closing";
                        yazi(cümle, speech);
                        frm3.groupBox1.Visible = false;
                    }
                    else if(frm3.groupBox1.Visible==false)
                    {
                        frm3.listBox1.Items.Clear();
                        string dosya_yolu = @"PhoneList.txt";
                        FileStream fs = new FileStream(dosya_yolu, FileMode.Open, FileAccess.Read);
                        StreamReader sw = new StreamReader(fs);
                        string yazi = sw.ReadLine();
                        while (yazi != null)
                        {
                            frm3.listBox1.Items.Add(yazi);
                            yazi = sw.ReadLine();
                        }
                        sw.Close();
                        fs.Close();
                        cümle = "Phone List is opening";
                        simon.SpeakAsync(cümle);
                        frm3.groupBox1.Visible = true;
                    }
                }
                else
                {
                    cümle = "Costumer service is Disable";
                    simon.SpeakAsync(cümle);
                }
            }
            if(speech=="be quiet")//---------------------------------------------------------------
            {
                if (frm3.Visible == true)
                {
                    cümle = "ok i'm silent";
                    yazi(cümle, speech);
                    frm3.axWindowsMediaPlayer1.Ctlcontrols.pause();                    
                }
                else
                {
                    cümle = "Costumer service is Disable";
                    yazi(cümle, speech);
                }
            }
            if (speech == "keep going")//---------------------------------------------------------------
            {
                if (frm3.Visible == true)
                {
                    frm3.axWindowsMediaPlayer1.Ctlcontrols.play();
                }
                else
                {
                    cümle = "Costumer service is Disable";
                    yazi(cümle, speech);
                }
            }
            if(speech == "Security Mode")// ---------------------------------------------------------------
            {
                cümle = "Security Mode Active";
                yazi(cümle, speech);
                LockWorkStation();
            }
            if(speech=="Fun Mode")// ---------------------------------------------------------------
            {
                if(groupBox1.Visible==true)
                {
                    cümle = "Fun Mode active";
                    yazi(cümle, speech);
                    this.axWindowsMediaPlayer1.fullScreen = true;                    
                }
            }
            if(speech=="close fun mode")// ---------------------------------------------------------------
            {
                if(this.axWindowsMediaPlayer1.fullScreen==true)
                {
                    cümle = "Fun Mode Deactive";
                    yazi(cümle, speech);
                    this.axWindowsMediaPlayer1.fullScreen = false;                  
                }
                else
                {
                    cümle = "Fun Mode Already Deactive";
                    yazi(cümle, speech); ;                   
                }                
            }
            if(speech=="Grace")// ---------------------------------------------------------------
            {
                cümle = "i'm listening sir";
                yazi(cümle, speech);

            }
            if (speech == "What is you name")// ---------------------------------------------------------------
            {
                cümle = "My name is Grace";
                yazi(cümle, speech);

            }
            if(speech=="Application Close")
            {
                cümle = "Good bye sir!";
                yazi(cümle, speech);
                Application.Exit();
            }
        }
        private void _recognizer_SpeechRecognized(object sender, SpeechDetectedEventArgs e)
        {
            RecTimeout = 0;
        }
        private void startlistening_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;
            if (speech == "Wake up")
            {
                Form2 frm2 = new Form2();
                frm2.Show();
                listBox1.Items.Add(DateTime.Now.ToString("hh:mm:ss") + "-> " + speech);
                startlistening.RecognizeAsyncCancel();
                simon.SpeakAsync("sir! i'm listening");
                _recognizer.RecognizeAsync(RecognizeMode.Multiple);
                listBox2.Items.Add(DateTime.Now.ToString("hh:mm:ss") + "-> " + "sir! i'm listening");
            }
        }

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)//gereksiz
        {
      
        }

        private void TmrSpeaking_Tick(object sender, EventArgs e)
        {
            if (RecTimeout == 10)
            {
                _recognizer.RecognizeAsyncCancel();

            }
            else if (RecTimeout == 11)
            {
                TmrSpeaking.Stop();
                startlistening.RecognizeAsync(RecognizeMode.Multiple);
                RecTimeout = 0;
            }
        }
    }
}