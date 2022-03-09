using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Xml;
using System.Threading;
using System.IO;

namespace Kur_Bilgisi
{

    public partial class exchange : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]

        private static extern IntPtr CreateRoundRectRgn(int NLeftRect
            ,int NRightRect,int NTopRect,int NBottomRect,int NWidthEllipse,int NHeightEllipse);



        //formu ekranda sürüklemek için
        int x = 0;
        int y = 0;
        bool location = false;
        string dosya = "FirstCoin.txt";//ilk sorgulanacak para birimi
        string ParaBirim;
        public exchange()
        {
            InitializeComponent();
            System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
            PnlNav.Height = button1.Height;
            PnlNav.Top = button1.Top;
            PnlNav.Left = button1.Left;
            button1.BackColor = Color.FromArgb(46, 51, 73);

            label2.Text = "Dashboard";
            this.pnlLoader.Controls.Clear();
            frmdashboard.FormBorderStyle = FormBorderStyle.None;
            this.pnlLoader.Controls.Add(frmdashboard);
            frmdashboard.Show();

            //Dosya yoksa Oluştur
            if (!File.Exists(dosya))
            {
                File.Create(dosya);

            }

            //dosyayı okuma modunda açıyoruz
            FileStream fileStream = new FileStream(dosya, FileMode.OpenOrCreate, FileAccess.Read);
            //dosyadan satır satır okuyup textBox içine yazıdırıyoruz
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string satir = reader.ReadLine();
                ParaBirim = satir;
                reader.Close();
            }
            fileStream.Close();
        }

     
        private void Form1_Load(object sender, EventArgs e)
        {
            //money
            string[] Items = { "USD","AUD","DKK","EUR","GBP","CHF","SEK","CAD","KWD","NOK","SAR","JPY","BGN","RON","RUB",
            "IRR","CNY","PKR","QAR","KRW","AZN","AED","XDR"};
            metroComboBox1.Items.AddRange(Items);
            Thread.Sleep(2000);
            if (ParaBirim == "USD")
            {
                Thread td = new Thread(new ThreadStart(Today));
                td.Start();
                Thread yd = new Thread(new ThreadStart(YesterDay));
                yd.Start();
            }
            else
            {
                metroComboBox1.SelectedIndex = metroComboBox1.Items.IndexOf(ParaBirim);
            }
        }
        //today's data
        public void Today()
        {
            Thread.Sleep(1000);
            string TodayData = "https://www.tcmb.gov.tr/kurlar/today.xml";
            var xmldoc = new XmlDocument();
            xmldoc.Load(TodayData);
            DateTime date = Convert.ToDateTime(xmldoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
            string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/ForexBuying").InnerXml;
            string usd2 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/ForexSelling").InnerXml;
            string usd3 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/BanknoteBuying").InnerXml;
            string usd4 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/BanknoteSelling").InnerXml;
            frmdashboard.label4.Text = "$" + usd;
            frmdashboard.label7.Text = "$" + usd2;
            frmdashboard.label10.Text = "$" + usd3;
            frmdashboard.label13.Text = "$" + usd4;
            frmdashboard.label5.Text = string.Format("{0}", date.ToShortDateString());
            frmdashboard.label6.Text = string.Format("{0}", date.ToShortDateString());
            frmdashboard.label9.Text = string.Format("{0}", date.ToShortDateString());
            frmdashboard.label12.Text = string.Format("{0}", date.ToShortDateString());
            frmdashboard.label15.Text = "US DOLLAR";
         
        }
        //dun ki kur verileri
        public void YesterDay()
        {

            try
            {
                Thread.Sleep(2000);
                //degiskenlere tarihi atıyoruz
                int years, month, day;
                day = DateTime.Now.Day;
                month = DateTime.Now.Month;
                years = DateTime.Now.Year;
                string url;
                url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                 Convert.ToString(month) + "/" + Convert.ToString(day - 1) +
                 Convert.ToString(month) + Convert.ToString(years) + ".xml";
                if (month < 10)
                {
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                 Convert.ToString(month) + "/" + Convert.ToString(day - 1) + "0" +
                 Convert.ToString(month) + Convert.ToString(years) + ".xml";
                }
                if (day < 10)
                {
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                 Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) +
                 Convert.ToString(month) + Convert.ToString(years) + ".xml";
                }
                if (month < 10 && day < 10)
                {
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                 Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) + "0" +
                 Convert.ToString(month) + Convert.ToString(years) + ".xml";
                }
                //dunku kur verilerini almak icin url degisimi - bugun -1


                //degisen urlyi yazıyoruz
                string TodayData = url;
                var xmldoc = new XmlDocument();
                xmldoc.Load(TodayData);
                DateTime date = Convert.ToDateTime(xmldoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/ForexBuying").InnerXml;
                string usd2 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/ForexSelling").InnerXml;
                string usd3 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/BanknoteBuying").InnerXml;
                string usd4 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/BanknoteSelling").InnerXml;
                frmYesterday.label4.Text = "$" + usd;
                frmYesterday.label7.Text = "$" + usd2;
                frmYesterday.label10.Text = "$" + usd3;
                frmYesterday.label13.Text = "$" + usd4;
                frmYesterday.label5.Text = string.Format("{0}", date.ToShortDateString());
                frmYesterday.label6.Text = string.Format("{0}", date.ToShortDateString());
                frmYesterday.label9.Text = string.Format("{0}", date.ToShortDateString());
                frmYesterday.label12.Text = string.Format("{0}", date.ToShortDateString());
                frmYesterday.label15.Text = "US DOLLAR";
            }
            catch (Exception)
            {

                frmYesterday.label1.Visible = true;
                frmYesterday.label2.Visible = true;
                frmYesterday.pictureBox1.Visible = true;
                frmYesterday.label4.Text = "$0";
                frmYesterday.label7.Text = "$0";
                frmYesterday.label10.Text = "$0";
                frmYesterday.label13.Text = "$0";

            }
        }
        //Forms
        frmDashboard frmdashboard = new frmDashboard() { Dock = DockStyle.Fill,TopLevel=false,TopMost=true};
        analysis frmanalysis = new analysis() { Dock = DockStyle.Fill,TopLevel=false,TopMost=true};
        Yesterday frmYesterday = new Yesterday() { Dock = DockStyle.Fill,TopLevel=false,TopMost=true};
        about frmAbout = new about() { Dock = DockStyle.Fill,TopLevel=false,TopMost=true};
        settings frmSettings = new settings() { Dock = DockStyle.Fill,TopLevel=false,TopMost=true};
        
        //Dashboard
        private void button1_Click(object sender, EventArgs e)
        {
            metroComboBox1.Visible = true;
            label16.Visible = true;
            PnlNav.Height = button1.Height;
            PnlNav.Top = button1.Top;
            PnlNav.Left = button1.Left;
            button1.BackColor = Color.FromArgb(46, 51, 73);
            label2.Text = "Dashboard";
            this.pnlLoader.Controls.Clear();
            frmdashboard.FormBorderStyle = FormBorderStyle.None;
            this.pnlLoader.Controls.Add(frmdashboard);
            frmdashboard.Show();

        }
        //analysis
        private void button2_Click(object sender, EventArgs e)
        {
            metroComboBox1.Visible = false;
            label16.Visible = false;
            PnlNav.Height = button2.Height;
            PnlNav.Top = button2.Top;
            button2.BackColor = Color.FromArgb(46, 51, 73);
            label2.Text = "Analysis";
            this.pnlLoader.Controls.Clear();
            frmanalysis.FormBorderStyle = FormBorderStyle.None;
            this.pnlLoader.Controls.Add(frmanalysis);
            frmanalysis.Show();
        }
        //yesterday
        private void button3_Click(object sender, EventArgs e)
        {
            metroComboBox1.Visible = true;
            label16.Visible = true;
            PnlNav.Height = button3.Height;
            PnlNav.Top = button3.Top;
            button3.BackColor = Color.FromArgb(46, 51, 73);
            label2.Text = "Yesterday";
            this.pnlLoader.Controls.Clear();
            frmYesterday.FormBorderStyle = FormBorderStyle.None;
            this.pnlLoader.Controls.Add(frmYesterday);
            frmYesterday.Show();
        }
        //about
        private void button4_Click(object sender, EventArgs e)
        {
            metroComboBox1.Visible = false;
            label16.Visible = false;
            PnlNav.Height = button4.Height;
            PnlNav.Top = button4.Top;
            button4.BackColor = Color.FromArgb(46, 51, 73);
            label2.Text = "About";
            this.pnlLoader.Controls.Clear();
            frmAbout.FormBorderStyle = FormBorderStyle.None;
            this.pnlLoader.Controls.Add(frmAbout);
            frmAbout.Show();
        }
        //settings
        private void button5_Click(object sender, EventArgs e)
        {
            metroComboBox1.Visible = false;
            label16.Visible = false;
            PnlNav.Height = button5.Height;
            PnlNav.Top = button5.Top;
            button5.BackColor = Color.FromArgb(46, 51, 73);
            label2.Text = "Settings";
            this.pnlLoader.Controls.Clear();
            frmSettings.FormBorderStyle = FormBorderStyle.None;
            this.pnlLoader.Controls.Add(frmSettings);
            frmSettings.Show();
        }

        private void button1_Leave(object sender, EventArgs e)
        {
            button1.BackColor = Color.FromArgb(24, 30, 54);
        }

        private void button2_Leave(object sender, EventArgs e)
        {
            button2.BackColor = Color.FromArgb(24, 30, 54);

        }

        private void button3_Leave(object sender, EventArgs e)
        {
            button3.BackColor = Color.FromArgb(24, 30, 54);

        }

        private void button4_Leave(object sender, EventArgs e)
        {
            button4.BackColor = Color.FromArgb(24, 30, 54);

        }

        private void button5_Leave(object sender, EventArgs e)
        {
            button5.BackColor = Color.FromArgb(24, 30, 54);

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(metroComboBox1.Text == "USD")
            {
                Today();
                YesterDay();
            }
            else if(metroComboBox1.Text == "AUD")
            {
                string TodayData = "https://www.tcmb.gov.tr/kurlar/today.xml";
                var xmldoc = new XmlDocument();
                xmldoc.Load(TodayData);
                DateTime date = Convert.ToDateTime(xmldoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='AUD']/ForexBuying").InnerXml;
                string usd2 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='AUD']/ForexSelling").InnerXml;
                string usd3 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='AUD']/BanknoteBuying").InnerXml;
                string usd4 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='AUD']/BanknoteSelling").InnerXml;
                frmdashboard.label4.Text = "$" + usd;
                frmdashboard.label7.Text = "$" + usd2;
                frmdashboard.label10.Text = "$" + usd3;
                frmdashboard.label13.Text = "$" + usd4;
                frmdashboard.label5.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label6.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label9.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label12.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label15.Text = "AUSTRALIAN DOLLAR";
                Thread.Sleep(1000);
                //-------------------------------------------------------------------
                //YESTERDAY DATA
                //degiskenlere tarihi atıyoruz
                try
                {
                    int years, month, day;
                    day = DateTime.Now.Day;
                    month = DateTime.Now.Month;
                    years = DateTime.Now.Year;
                    string url;
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (month < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (month < 10 && day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    //dunku kur verilerini almak icin url degisimi - bugun -1


                    //degisen urlyi yazıyoruz
                    string yesterData = url;
                    var xmldocY = new XmlDocument();
                    xmldocY.Load(yesterData);
                    DateTime date2 = Convert.ToDateTime(xmldocY.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                    string yester1 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='AUD']/ForexBuying").InnerXml;
                    string yester2 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='AUD']/ForexSelling").InnerXml;
                    string yester3 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='AUD']/BanknoteBuying").InnerXml;
                    string yester4 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='AUD']/BanknoteSelling").InnerXml;
                    frmYesterday.label4.Text = "$" + yester1;
                    frmYesterday.label7.Text = "$" + yester2;
                    frmYesterday.label10.Text = "$" + yester3;
                    frmYesterday.label13.Text = "$" + yester4;
                    frmYesterday.label5.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label6.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label9.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label12.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label15.Text = "AUSTRALIAN DOLLAR";
                }
                catch (Exception)
                {

                    frmYesterday.label1.Visible = true;
                    frmYesterday.label2.Visible = true;
                    frmYesterday.pictureBox1.Visible = true;
                    frmYesterday.label4.Text = "$0";
                    frmYesterday.label7.Text = "$0";
                    frmYesterday.label10.Text = "$0";
                    frmYesterday.label13.Text = "$0";
                }
             
             
            }
            else if (metroComboBox1.Text == "DKK")
            {
                string TodayData = "https://www.tcmb.gov.tr/kurlar/today.xml";
                var xmldoc = new XmlDocument();
                xmldoc.Load(TodayData);
                DateTime date = Convert.ToDateTime(xmldoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='DKK']/ForexBuying").InnerXml;
                string usd2 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='DKK']/ForexSelling").InnerXml;
                string usd3 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='DKK']/BanknoteBuying").InnerXml;
                string usd4 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='DKK']/BanknoteSelling").InnerXml;
                frmdashboard.label4.Text = "D" + usd;
                frmdashboard.label7.Text = "D" + usd2;
                frmdashboard.label10.Text = "D" + usd3;
                frmdashboard.label13.Text = "D" + usd4;
                frmdashboard.label5.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label6.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label9.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label12.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label15.Text = "DANISH KRONE";

                Thread.Sleep(1000);
                //-------------------------------------------------------------------
                //YESTERDAY DATA
                //degiskenlere tarihi atıyoruz
                try
                {
                    int years, month, day;
                    day = DateTime.Now.Day;
                    month = DateTime.Now.Month;
                    years = DateTime.Now.Year;
                    string url;
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (month < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (month < 10 && day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    //dunku kur verilerini almak icin url degisimi - bugun -1


                    //degisen urlyi yazıyoruz
                    string yesterData = url;
                    var xmldocY = new XmlDocument();
                    xmldocY.Load(yesterData);
                    DateTime date2 = Convert.ToDateTime(xmldocY.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                    string yester1 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='DKK']/ForexBuying").InnerXml;
                    string yester2 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='DKK']/ForexSelling").InnerXml;
                    string yester3 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='DKK']/BanknoteBuying").InnerXml;
                    string yester4 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='DKK']/BanknoteSelling").InnerXml;
                    frmYesterday.label4.Text = "D" + yester1;
                    frmYesterday.label7.Text = "D" + yester2;
                    frmYesterday.label10.Text = "D" + yester3;
                    frmYesterday.label13.Text = "$" + yester4;
                    frmYesterday.label5.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label6.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label9.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label12.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label15.Text = "DANISH KRONE";
                }
                catch (Exception)
                {

                    frmYesterday.label1.Visible = true;
                    frmYesterday.label2.Visible = true;
                    frmYesterday.pictureBox1.Visible = true;
                    frmYesterday.label4.Text = "D0";
                    frmYesterday.label7.Text = "D0";
                    frmYesterday.label10.Text = "D0";
                    frmYesterday.label13.Text = "D0";
                }

            }
            else if (metroComboBox1.Text == "EUR")
            {
                string TodayData = "https://www.tcmb.gov.tr/kurlar/today.xml";
                var xmldoc = new XmlDocument();
                xmldoc.Load(TodayData);
                DateTime date = Convert.ToDateTime(xmldoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='EUR']/ForexBuying").InnerXml;
                string usd2 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='EUR']/ForexSelling").InnerXml;
                string usd3 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='EUR']/BanknoteBuying").InnerXml;
                string usd4 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='EUR']/BanknoteSelling").InnerXml;
                frmdashboard.label4.Text = "€" + usd;
                frmdashboard.label7.Text = "€" + usd2;
                frmdashboard.label10.Text = "€" + usd3;
                frmdashboard.label13.Text = "€" + usd4;
                frmdashboard.label5.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label6.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label9.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label12.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label15.Text = "EURO";
                Thread.Sleep(1000);
                //-------------------------------------------------------------------
                //YESTERDAY DATA
                //degiskenlere tarihi atıyoruz
                try
                {
                    int years, month, day;
                    day = DateTime.Now.Day;
                    month = DateTime.Now.Month;
                    years = DateTime.Now.Year;
                    string url;
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (month < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (month < 10 && day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    //dunku kur verilerini almak icin url degisimi - bugun -1


                    //degisen urlyi yazıyoruz
                    string yesterData = url;
                    var xmldocY = new XmlDocument();
                    xmldocY.Load(yesterData);
                    DateTime date2 = Convert.ToDateTime(xmldocY.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                    string yester1 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='EUR']/ForexBuying").InnerXml;
                    string yester2 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='EUR']/ForexSelling").InnerXml;
                    string yester3 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='EUR']/BanknoteBuying").InnerXml;
                    string yester4 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='EUR']/BanknoteSelling").InnerXml;
                    frmYesterday.label4.Text = "€" + yester1;
                    frmYesterday.label7.Text = "€" + yester2;
                    frmYesterday.label10.Text = "€" + yester3;
                    frmYesterday.label13.Text = "€" + yester4;
                    frmYesterday.label5.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label6.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label9.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label12.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label15.Text = "EURO";
                }
                catch (Exception)
                {

                    frmYesterday.label1.Visible = true;
                    frmYesterday.label2.Visible = true;
                    frmYesterday.pictureBox1.Visible = true;
                    frmYesterday.label4.Text = "€0";
                    frmYesterday.label7.Text = "€0";
                    frmYesterday.label10.Text = "€0";
                    frmYesterday.label13.Text = "€0";
                }

            }
            else if (metroComboBox1.Text == "GBP")
            {
                string TodayData = "https://www.tcmb.gov.tr/kurlar/today.xml";
                var xmldoc = new XmlDocument();
                xmldoc.Load(TodayData);
                DateTime date = Convert.ToDateTime(xmldoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='GBP']/ForexBuying").InnerXml;
                string usd2 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='GBP']/ForexSelling").InnerXml;
                string usd3 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='GBP']/BanknoteBuying").InnerXml;
                string usd4 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='GBP']/BanknoteSelling").InnerXml;
                frmdashboard.label4.Text = "£" + usd;
                frmdashboard.label7.Text = "£" + usd2;
                frmdashboard.label10.Text = "£" + usd3;
                frmdashboard.label13.Text = "£" + usd4;
                frmdashboard.label5.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label6.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label9.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label12.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label15.Text = "POUND STERLING";

                Thread.Sleep(1000);
                //-------------------------------------------------------------------
                //YESTERDAY DATA
                //degiskenlere tarihi atıyoruz
                try
                {
                    int years, month, day;
                    day = DateTime.Now.Day;
                    month = DateTime.Now.Month;
                    years = DateTime.Now.Year;
                    string url;
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (month < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (month < 10 && day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    //dunku kur verilerini almak icin url degisimi - bugun -1


                    //degisen urlyi yazıyoruz
                    string yesterData = url;
                    var xmldocY = new XmlDocument();
                    xmldocY.Load(yesterData);
                    DateTime date2 = Convert.ToDateTime(xmldocY.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                    string yester1 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='GBP']/ForexBuying").InnerXml;
                    string yester2 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='GBP']/ForexSelling").InnerXml;
                    string yester3 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='GBP']/BanknoteBuying").InnerXml;
                    string yester4 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='GBP']/BanknoteSelling").InnerXml;
                    frmYesterday.label4.Text = "£" + yester1;
                    frmYesterday.label7.Text = "£" + yester2;
                    frmYesterday.label10.Text = "£" + yester3;
                    frmYesterday.label13.Text = "£" + yester4;
                    frmYesterday.label5.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label6.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label9.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label12.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label15.Text = "POUND STERLING";
                }
                catch (Exception)
                {

                    frmYesterday.label1.Visible = true;
                    frmYesterday.label2.Visible = true;
                    frmYesterday.pictureBox1.Visible = true;
                    frmYesterday.label4.Text = "£0";
                    frmYesterday.label7.Text = "£0";
                    frmYesterday.label10.Text = "£0";
                    frmYesterday.label13.Text = "£0";
                }

            }
            else if (metroComboBox1.Text == "CHF")
            {
                string TodayData = "https://www.tcmb.gov.tr/kurlar/today.xml";
                var xmldoc = new XmlDocument();
                xmldoc.Load(TodayData);
                DateTime date = Convert.ToDateTime(xmldoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='CHF']/ForexBuying").InnerXml;
                string usd2 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='CHF']/ForexSelling").InnerXml;
                string usd3 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='CHF']/BanknoteBuying").InnerXml;
                string usd4 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='CHF']/BanknoteSelling").InnerXml;
                frmdashboard.label4.Text = "F" + usd;
                frmdashboard.label7.Text = "F" + usd2;
                frmdashboard.label10.Text = "F" + usd3;
                frmdashboard.label13.Text = "F" + usd4;
                frmdashboard.label5.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label6.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label9.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label12.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label15.Text = "SWISS FRANK";

                Thread.Sleep(1000);
                //-------------------------------------------------------------------
                //YESTERDAY DATA
                //degiskenlere tarihi atıyoruz
                try
                {
                    int years, month, day;
                    day = DateTime.Now.Day;
                    month = DateTime.Now.Month;
                    years = DateTime.Now.Year;
                    string url;
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (month < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (month < 10 && day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    //dunku kur verilerini almak icin url degisimi - bugun -1


                    //degisen urlyi yazıyoruz
                    string yesterData = url;
                    var xmldocY = new XmlDocument();
                    xmldocY.Load(yesterData);
                    DateTime date2 = Convert.ToDateTime(xmldocY.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                    string yester1 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='CHF']/ForexBuying").InnerXml;
                    string yester2 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='CHF']/ForexSelling").InnerXml;
                    string yester3 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='CHF']/BanknoteBuying").InnerXml;
                    string yester4 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='CHF']/BanknoteSelling").InnerXml;
                    frmYesterday.label4.Text = "F" + yester1;
                    frmYesterday.label7.Text = "F" + yester2;
                    frmYesterday.label10.Text = "F" + yester3;
                    frmYesterday.label13.Text = "F" + yester4;
                    frmYesterday.label5.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label6.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label9.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label12.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label15.Text = "SWISS FRANK";
                }
                catch (Exception)
                {

                    frmYesterday.label1.Visible = true;
                    frmYesterday.label2.Visible = true;
                    frmYesterday.pictureBox1.Visible = true;
                    frmYesterday.label4.Text = "F0";
                    frmYesterday.label7.Text = "F0";
                    frmYesterday.label10.Text = "F0";
                    frmYesterday.label13.Text = "F0";
                }

            }
            else if (metroComboBox1.Text == "SEK")
            {
                string TodayData = "https://www.tcmb.gov.tr/kurlar/today.xml";
                var xmldoc = new XmlDocument();
                xmldoc.Load(TodayData);
                DateTime date = Convert.ToDateTime(xmldoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='SEK']/ForexBuying").InnerXml;
                string usd2 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='SEK']/ForexSelling").InnerXml;
                string usd3 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='SEK']/BanknoteBuying").InnerXml;
                string usd4 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='SEK']/BanknoteSelling").InnerXml;
                frmdashboard.label4.Text = "SK" + usd;
                frmdashboard.label7.Text = "SK" + usd2;
                frmdashboard.label10.Text = "SK" + usd3;
                frmdashboard.label13.Text = "SK" + usd4;
                frmdashboard.label5.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label6.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label9.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label12.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label15.Text = "SWEDISH KRONA";

                Thread.Sleep(1000);
                //-------------------------------------------------------------------
                //YESTERDAY DATA
                //degiskenlere tarihi atıyoruz
                try
                {
                    int years, month, day;
                    day = DateTime.Now.Day;
                    month = DateTime.Now.Month;
                    years = DateTime.Now.Year;
                    string url;
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (month < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (month < 10 && day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    //dunku kur verilerini almak icin url degisimi - bugun -1


                    //degisen urlyi yazıyoruz
                    string yesterData = url;
                    var xmldocY = new XmlDocument();
                    xmldocY.Load(yesterData);
                    DateTime date2 = Convert.ToDateTime(xmldocY.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                    string yester1 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='SEK']/ForexBuying").InnerXml;
                    string yester2 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='SEK']/ForexSelling").InnerXml;
                    string yester3 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='SEK']/BanknoteBuying").InnerXml;
                    string yester4 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='SEK']/BanknoteSelling").InnerXml;
                    frmYesterday.label4.Text = "SK" + yester1;
                    frmYesterday.label7.Text = "SK" + yester2;
                    frmYesterday.label10.Text = "SK" + yester3;
                    frmYesterday.label13.Text = "SK" + yester4;
                    frmYesterday.label5.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label6.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label9.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label12.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label15.Text = "SWEDISH KRONE";
                }
                catch (Exception)
                {

                    frmYesterday.label1.Visible = true;
                    frmYesterday.label2.Visible = true;
                    frmYesterday.pictureBox1.Visible = true;
                    frmYesterday.label4.Text = "SK0";
                    frmYesterday.label7.Text = "SK0";
                    frmYesterday.label10.Text = "SK0";
                    frmYesterday.label13.Text = "SK0";
                }

            }
            else if (metroComboBox1.Text == "CAD")
            {
                string TodayData = "https://www.tcmb.gov.tr/kurlar/today.xml";
                var xmldoc = new XmlDocument();
                xmldoc.Load(TodayData);
                DateTime date = Convert.ToDateTime(xmldoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='CAD']/ForexBuying").InnerXml;
                string usd2 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='CAD']/ForexSelling").InnerXml;
                string usd3 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='CAD']/BanknoteBuying").InnerXml;
                string usd4 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='CAD']/BanknoteSelling").InnerXml;
                frmdashboard.label4.Text = "C$" + usd;
                frmdashboard.label7.Text = "C$" + usd2;
                frmdashboard.label10.Text = "C$" + usd3;
                frmdashboard.label13.Text = "C$" + usd4;
                frmdashboard.label5.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label6.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label9.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label12.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label15.Text = "CANADIAN DOLLAR";

                Thread.Sleep(1000);

                //-------------------------------------------------------------------
                //YESTERDAY DATA
                //degiskenlere tarihi atıyoruz
                try
                {
                    int years, month, day;
                    day = DateTime.Now.Day;
                    month = DateTime.Now.Month;
                    years = DateTime.Now.Year;
                    string url;
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (month < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (month < 10 && day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    //dunku kur verilerini almak icin url degisimi - bugun -1


                    //degisen urlyi yazıyoruz
                    string yesterData = url;
                    var xmldocY = new XmlDocument();
                    xmldocY.Load(yesterData);
                    DateTime date2 = Convert.ToDateTime(xmldocY.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                    string yester1 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='CAD']/ForexBuying").InnerXml;
                    string yester2 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='CAD']/ForexSelling").InnerXml;
                    string yester3 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='CAD']/BanknoteBuying").InnerXml;
                    string yester4 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='CAD']/BanknoteSelling").InnerXml;
                    frmYesterday.label4.Text = "C$" + yester1;
                    frmYesterday.label7.Text = "C$" + yester2;
                    frmYesterday.label10.Text = "C$" + yester3;
                    frmYesterday.label13.Text = "C$" + yester4;
                    frmYesterday.label5.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label6.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label9.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label12.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label15.Text = "CANADIAN DOLLAR";
                }
                catch (Exception)
                {

                    frmYesterday.label1.Visible = true;
                    frmYesterday.label2.Visible = true;
                    frmYesterday.pictureBox1.Visible = true;
                    frmYesterday.label4.Text = "C$0";
                    frmYesterday.label7.Text = "C$0";
                    frmYesterday.label10.Text = "C$0";
                    frmYesterday.label13.Text = "C$0";
                }

            }
            else if (metroComboBox1.Text == "KWD")
            {
                string TodayData = "https://www.tcmb.gov.tr/kurlar/today.xml";
                var xmldoc = new XmlDocument();
                xmldoc.Load(TodayData);
                DateTime date = Convert.ToDateTime(xmldoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='KWD']/ForexBuying").InnerXml;
                string usd2 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='KWD']/ForexSelling").InnerXml;
                string usd3 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='KWD']/BanknoteBuying").InnerXml;
                string usd4 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='KWD']/BanknoteSelling").InnerXml;
                frmdashboard.label4.Text = "KW" + usd;
                frmdashboard.label7.Text = "KW" + usd2;
                frmdashboard.label10.Text = "KW" + usd3;
                frmdashboard.label13.Text = "KW" + usd4;
                frmdashboard.label5.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label6.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label9.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label12.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label15.Text = "KUWAITI DINAR";

                Thread.Sleep(1000);

                //-------------------------------------------------------------------
                //YESTERDAY DATA
                //degiskenlere tarihi atıyoruz
                try
                {
                    int years, month, day;
                    day = DateTime.Now.Day;
                    month = DateTime.Now.Month;
                    years = DateTime.Now.Year;
                    string url;
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (month < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (month < 10 && day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    //dunku kur verilerini almak icin url degisimi - bugun -1


                    //degisen urlyi yazıyoruz
                    string yesterData = url;
                    var xmldocY = new XmlDocument();
                    xmldocY.Load(yesterData);
                    DateTime date2 = Convert.ToDateTime(xmldocY.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                    string yester1 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='KWD']/ForexBuying").InnerXml;
                    string yester2 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='KWD']/ForexSelling").InnerXml;
                    string yester3 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='KWD']/BanknoteBuying").InnerXml;
                    string yester4 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='KWD']/BanknoteSelling").InnerXml;
                    frmYesterday.label4.Text = "KW" + yester1;
                    frmYesterday.label7.Text = "KW" + yester2;
                    frmYesterday.label10.Text = "KW" + yester3;
                    frmYesterday.label13.Text = "KW" + yester4;
                    frmYesterday.label5.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label6.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label9.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label12.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label15.Text = "KUWAITI DINAR";
                }
                catch (Exception)
                {

                    frmYesterday.label1.Visible = true;
                    frmYesterday.label2.Visible = true;
                    frmYesterday.pictureBox1.Visible = true;
                    frmYesterday.label4.Text = "KW0";
                    frmYesterday.label7.Text = "KW0";
                    frmYesterday.label10.Text = "KW0";
                    frmYesterday.label13.Text = "KW0";
                }

            }
            else if (metroComboBox1.Text == "NOK")
            {
                string TodayData = "https://www.tcmb.gov.tr/kurlar/today.xml";
                var xmldoc = new XmlDocument();
                xmldoc.Load(TodayData);
                DateTime date = Convert.ToDateTime(xmldoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='NOK']/ForexBuying").InnerXml;
                string usd2 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='NOK']/ForexSelling").InnerXml;
                string usd3 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='NOK']/BanknoteBuying").InnerXml;
                string usd4 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='NOK']/BanknoteSelling").InnerXml;
                frmdashboard.label4.Text = "KR" + usd;
                frmdashboard.label7.Text = "KR" + usd2;
                frmdashboard.label10.Text = "KR" + usd3;
                frmdashboard.label13.Text = "KR" + usd4;
                frmdashboard.label5.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label6.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label9.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label12.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label15.Text = "NORWEGIAN KRONE";

                Thread.Sleep(1000);

                //-------------------------------------------------------------------
                //YESTERDAY DATA
                //degiskenlere tarihi atıyoruz
                try
                {
                    int years, month, day;
                    day = DateTime.Now.Day;
                    month = DateTime.Now.Month;
                    years = DateTime.Now.Year;
                    string url;
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (month < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (month < 10 && day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    //dunku kur verilerini almak icin url degisimi - bugun -1


                    //degisen urlyi yazıyoruz
                    string yesterData = url;
                    var xmldocY = new XmlDocument();
                    xmldocY.Load(yesterData);
                    DateTime date2 = Convert.ToDateTime(xmldocY.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                    string yester1 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='NOK']/ForexBuying").InnerXml;
                    string yester2 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='NOK']/ForexSelling").InnerXml;
                    string yester3 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='NOK']/BanknoteBuying").InnerXml;
                    string yester4 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='NOK']/BanknoteSelling").InnerXml;
                    frmYesterday.label4.Text = "KR" + yester1;
                    frmYesterday.label7.Text = "KR" + yester2;
                    frmYesterday.label10.Text = "KR" + yester3;
                    frmYesterday.label13.Text = "KR" + yester4;
                    frmYesterday.label5.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label6.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label9.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label12.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label15.Text = "NORWEGIAN KRONE";
                }
                catch (Exception)
                {

                    frmYesterday.label1.Visible = true;
                    frmYesterday.label2.Visible = true;
                    frmYesterday.pictureBox1.Visible = true;
                    frmYesterday.label4.Text = "KR0";
                    frmYesterday.label7.Text = "KR0";
                    frmYesterday.label10.Text = "KR0";
                    frmYesterday.label13.Text = "KR0";
                }

            }
            else if (metroComboBox1.Text == "SAR")
            {
                string TodayData = "https://www.tcmb.gov.tr/kurlar/today.xml";
                var xmldoc = new XmlDocument();
                xmldoc.Load(TodayData);
                DateTime date = Convert.ToDateTime(xmldoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='SAR']/ForexBuying").InnerXml;
                string usd2 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='SAR']/ForexSelling").InnerXml;
                string usd3 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='SAR']/BanknoteBuying").InnerXml;
                string usd4 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='SAR']/BanknoteSelling").InnerXml;
                frmdashboard.label4.Text = "S" + usd;
                frmdashboard.label7.Text = "S" + usd2;
                frmdashboard.label10.Text = "S" + usd3;
                frmdashboard.label13.Text = "S" + usd4;
                frmdashboard.label5.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label6.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label9.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label12.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label15.Text = "SAUDI RIYAL";

                Thread.Sleep(1000);

                //-------------------------------------------------------------------
                //YESTERDAY DATA
                //degiskenlere tarihi atıyoruz
                try
                {
                    int years, month, day;
                    day = DateTime.Now.Day;
                    month = DateTime.Now.Month;
                    years = DateTime.Now.Year;
                    string url;
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (month < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (month < 10 && day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    //dunku kur verilerini almak icin url degisimi - bugun -1


                    //degisen urlyi yazıyoruz
                    string yesterData = url;
                    var xmldocY = new XmlDocument();
                    xmldocY.Load(yesterData);
                    DateTime date2 = Convert.ToDateTime(xmldocY.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                    string yester1 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='SAR']/ForexBuying").InnerXml;
                    string yester2 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='SAR']/ForexSelling").InnerXml;
                    string yester3 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='SAR']/BanknoteBuying").InnerXml;
                    string yester4 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='SAR']/BanknoteSelling").InnerXml;
                    frmYesterday.label4.Text = "S" + yester1;
                    frmYesterday.label7.Text = "S" + yester2;
                    frmYesterday.label10.Text = "S" + yester3;
                    frmYesterday.label13.Text = "S" + yester4;
                    frmYesterday.label5.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label6.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label9.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label12.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label15.Text = "SAUDI RIAL";
                }
                catch (Exception)
                {

                    frmYesterday.label1.Visible = true;
                    frmYesterday.label2.Visible = true;
                    frmYesterday.pictureBox1.Visible = true;
                    frmYesterday.label4.Text = "S0";
                    frmYesterday.label7.Text = "S0";
                    frmYesterday.label10.Text = "S0";
                    frmYesterday.label13.Text = "S0";
                }

            }
            else if (metroComboBox1.Text == "JPY")
            {
                string TodayData = "https://www.tcmb.gov.tr/kurlar/today.xml";
                var xmldoc = new XmlDocument();
                xmldoc.Load(TodayData);
                DateTime date = Convert.ToDateTime(xmldoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='JPY']/ForexBuying").InnerXml;
                string usd2 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='JPY']/ForexSelling").InnerXml;
                string usd3 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='JPY']/BanknoteBuying").InnerXml;
                string usd4 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='JPY']/BanknoteSelling").InnerXml;
                frmdashboard.label4.Text = "¥" + usd;
                frmdashboard.label7.Text = "¥" + usd2;
                frmdashboard.label10.Text = "¥" + usd3;
                frmdashboard.label13.Text = "¥" + usd4;
                frmdashboard.label5.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label6.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label9.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label12.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label15.Text = "JAPENESE YEN";

                Thread.Sleep(1000);

                //-------------------------------------------------------------------
                //YESTERDAY DATA
                //degiskenlere tarihi atıyoruz
                try
                {
                    int years, month, day;
                    day = DateTime.Now.Day;
                    month = DateTime.Now.Month;
                    years = DateTime.Now.Year;
                    string url;
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (month < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (month < 10 && day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    //dunku kur verilerini almak icin url degisimi - bugun -1


                    //degisen urlyi yazıyoruz
                    string yesterData = url;
                    var xmldocY = new XmlDocument();
                    xmldocY.Load(yesterData);
                    DateTime date2 = Convert.ToDateTime(xmldocY.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                    string yester1 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='JPY']/ForexBuying").InnerXml;
                    string yester2 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='JPY']/ForexSelling").InnerXml;
                    string yester3 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='JPY']/BanknoteBuying").InnerXml;
                    string yester4 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='JPY']/BanknoteSelling").InnerXml;
                    frmYesterday.label4.Text = "¥" + yester1;
                    frmYesterday.label7.Text = "¥" + yester2;
                    frmYesterday.label10.Text = "¥" + yester3;
                    frmYesterday.label13.Text = "¥" + yester4;
                    frmYesterday.label5.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label6.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label9.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label12.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label15.Text = "JAPENESE YEN";
                }
                catch (Exception)
                {

                    frmYesterday.label1.Visible = true;
                    frmYesterday.label2.Visible = true;
                    frmYesterday.pictureBox1.Visible = true;
                    frmYesterday.label4.Text = "¥0";
                    frmYesterday.label7.Text = "¥0";
                    frmYesterday.label10.Text = "¥0";
                    frmYesterday.label13.Text = "¥0";
                }

            }
            else if (metroComboBox1.Text == "BGN")
            {
                string TodayData = "https://www.tcmb.gov.tr/kurlar/today.xml";
                var xmldoc = new XmlDocument();
                xmldoc.Load(TodayData);
                DateTime date = Convert.ToDateTime(xmldoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='BGN']/ForexBuying").InnerXml;
                string usd2 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='BGN']/ForexSelling").InnerXml;
                string usd3 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='BGN']/BanknoteBuying").InnerXml;
                string usd4 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='BGN']/BanknoteSelling").InnerXml;
                frmdashboard.label4.Text = "BL" + usd;
                frmdashboard.label7.Text = "BL" + usd2;
                frmdashboard.label10.Text = "BL" + usd3;
                frmdashboard.label13.Text = "BL" + usd4;
                frmdashboard.label5.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label6.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label9.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label12.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label15.Text = "BULGARIAN LEV";

                Thread.Sleep(1000);

                //-------------------------------------------------------------------
                //YESTERDAY DATA
                //degiskenlere tarihi atıyoruz
                try
                {
                    int years, month, day;
                    day = DateTime.Now.Day;
                    month = DateTime.Now.Month;
                    years = DateTime.Now.Year;
                    string url;
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (month < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (month < 10 && day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    //dunku kur verilerini almak icin url degisimi - bugun -1


                    //degisen urlyi yazıyoruz
                    string yesterData = url;
                    var xmldocY = new XmlDocument();
                    xmldocY.Load(yesterData);
                    DateTime date2 = Convert.ToDateTime(xmldocY.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                    string yester1 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='BGN']/ForexBuying").InnerXml;
                    string yester2 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='BGN']/ForexSelling").InnerXml;
                    string yester3 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='BGN']/BanknoteBuying").InnerXml;
                    string yester4 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='BGN']/BanknoteSelling").InnerXml;
                    frmYesterday.label4.Text = "BL" + yester1;
                    frmYesterday.label7.Text = "BL" + yester2;
                    frmYesterday.label10.Text = "BL" + yester3;
                    frmYesterday.label13.Text = "BL" + yester4;
                    frmYesterday.label5.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label6.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label9.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label12.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label15.Text = "BULGARIAN LEV";
                }
                catch (Exception)
                {

                    frmYesterday.label1.Visible = true;
                    frmYesterday.label2.Visible = true;
                    frmYesterday.pictureBox1.Visible = true;
                    frmYesterday.label4.Text = "BL0";
                    frmYesterday.label7.Text = "BL0";
                    frmYesterday.label10.Text = "BL0";
                    frmYesterday.label13.Text = "BL0";
                }

            }
            else if (metroComboBox1.Text == "RON")
            {
                string TodayData = "https://www.tcmb.gov.tr/kurlar/today.xml";
                var xmldoc = new XmlDocument();
                xmldoc.Load(TodayData);
                DateTime date = Convert.ToDateTime(xmldoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='RON']/ForexBuying").InnerXml;
                string usd2 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='RON']/ForexSelling").InnerXml;
                string usd3 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='RON']/BanknoteBuying").InnerXml;
                string usd4 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='RON']/BanknoteSelling").InnerXml;
                frmdashboard.label4.Text = "RL" + usd;
                frmdashboard.label7.Text = "RL" + usd2;
                frmdashboard.label10.Text = "RL" + usd3;
                frmdashboard.label13.Text = "RL" + usd4;
                frmdashboard.label5.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label6.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label9.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label12.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label15.Text = "NEW LEU";

                Thread.Sleep(1000);

                //-------------------------------------------------------------------
                //YESTERDAY DATA
                //degiskenlere tarihi atıyoruz
                try
                {
                    int years, month, day;
                    day = DateTime.Now.Day;
                    month = DateTime.Now.Month;
                    years = DateTime.Now.Year;
                    string url;
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (month < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (month < 10 && day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    //dunku kur verilerini almak icin url degisimi - bugun -1


                    //degisen urlyi yazıyoruz
                    string yesterData = url;
                    var xmldocY = new XmlDocument();
                    xmldocY.Load(yesterData);
                    DateTime date2 = Convert.ToDateTime(xmldocY.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                    string yester1 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='RON']/ForexBuying").InnerXml;
                    string yester2 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='RON']/ForexSelling").InnerXml;
                    string yester3 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='RON']/BanknoteBuying").InnerXml;
                    string yester4 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='RON']/BanknoteSelling").InnerXml;
                    frmYesterday.label4.Text = "RL" + yester1;
                    frmYesterday.label7.Text = "RL" + yester2;
                    frmYesterday.label10.Text = "RL" + yester3;
                    frmYesterday.label13.Text = "RL" + yester4;
                    frmYesterday.label5.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label6.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label9.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label12.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label15.Text = "NEW LEU";
                }
                catch (Exception)
                {

                    frmYesterday.label1.Visible = true;
                    frmYesterday.label2.Visible = true;
                    frmYesterday.pictureBox1.Visible = true;
                    frmYesterday.label4.Text = "RL0";
                    frmYesterday.label7.Text = "RL0";
                    frmYesterday.label10.Text = "RL0";
                    frmYesterday.label13.Text = "RL0";
                }

            }
            else if (metroComboBox1.Text == "RUB")
            {
                string TodayData = "https://www.tcmb.gov.tr/kurlar/today.xml";
                var xmldoc = new XmlDocument();
                xmldoc.Load(TodayData);
                DateTime date = Convert.ToDateTime(xmldoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='RUB']/ForexBuying").InnerXml;
                string usd2 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='RUB']/ForexSelling").InnerXml;
                string usd3 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='RUB']/BanknoteBuying").InnerXml;
                string usd4 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='RUB']/BanknoteSelling").InnerXml;
                frmdashboard.label4.Text = "₽" + usd;
                frmdashboard.label7.Text = "₽" + usd2;
                frmdashboard.label10.Text = "₽" + usd3;
                frmdashboard.label13.Text = "₽" + usd4;
                frmdashboard.label5.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label6.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label9.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label12.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label15.Text = "RUSSIAN ROUBLE";

                Thread.Sleep(1000);

                //-------------------------------------------------------------------
                //YESTERDAY DATA
                //degiskenlere tarihi atıyoruz
                try
                {
                    int years, month, day;
                    day = DateTime.Now.Day;
                    month = DateTime.Now.Month;
                    years = DateTime.Now.Year;
                    string url;
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (month < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (month < 10 && day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    //dunku kur verilerini almak icin url degisimi - bugun -1


                    //degisen urlyi yazıyoruz
                    string yesterData = url;
                    var xmldocY = new XmlDocument();
                    xmldocY.Load(yesterData);
                    DateTime date2 = Convert.ToDateTime(xmldocY.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                    string yester1 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='RUB']/ForexBuying").InnerXml;
                    string yester2 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='RUB']/ForexSelling").InnerXml;
                    string yester3 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='RUB']/BanknoteBuying").InnerXml;
                    string yester4 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='RUB']/BanknoteSelling").InnerXml;
                    frmYesterday.label4.Text = "₽" + yester1;
                    frmYesterday.label7.Text = "₽" + yester2;
                    frmYesterday.label10.Text = "₽" + yester3;
                    frmYesterday.label13.Text = "₽" + yester4;
                    frmYesterday.label5.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label6.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label9.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label12.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label15.Text = "RUSSIAN ROUBLE";
                }
                catch (Exception)
                {

                    frmYesterday.label1.Visible = true;
                    frmYesterday.label2.Visible = true;
                    frmYesterday.pictureBox1.Visible = true;
                    frmYesterday.label4.Text = "₽0";
                    frmYesterday.label7.Text = "₽0";
                    frmYesterday.label10.Text = "₽0";
                    frmYesterday.label13.Text = "₽0";
                }

            }
            else if (metroComboBox1.Text == "IRR")
            {
                string TodayData = "https://www.tcmb.gov.tr/kurlar/today.xml";
                var xmldoc = new XmlDocument();
                xmldoc.Load(TodayData);
                DateTime date = Convert.ToDateTime(xmldoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='IRR']/ForexBuying").InnerXml;
                string usd2 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='IRR']/ForexSelling").InnerXml;
                string usd3 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='IRR']/BanknoteBuying").InnerXml;
                string usd4 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='IRR']/BanknoteSelling").InnerXml;
                frmdashboard.label4.Text = "IR" + usd;
                frmdashboard.label7.Text = "IR" + usd2;
                frmdashboard.label10.Text = "IR" + usd3;
                frmdashboard.label13.Text = "IR" + usd4;
                frmdashboard.label5.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label6.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label9.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label12.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label15.Text = "IRANIAN RIAL";

                Thread.Sleep(1000);

                //-------------------------------------------------------------------
                //YESTERDAY DATA
                //degiskenlere tarihi atıyoruz
                try
                {
                    int years, month, day;
                    day = DateTime.Now.Day;
                    month = DateTime.Now.Month;
                    years = DateTime.Now.Year;
                    string url;
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (month < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (month < 10 && day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    //dunku kur verilerini almak icin url degisimi - bugun -1


                    //degisen urlyi yazıyoruz
                    string yesterData = url;
                    var xmldocY = new XmlDocument();
                    xmldocY.Load(yesterData);
                    DateTime date2 = Convert.ToDateTime(xmldocY.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                    string yester1 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='IRR']/ForexBuying").InnerXml;
                    string yester2 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='IRR']/ForexSelling").InnerXml;
                    string yester3 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='IRR']/BanknoteBuying").InnerXml;
                    string yester4 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='IRR']/BanknoteSelling").InnerXml;
                    frmYesterday.label4.Text = "IR" + yester1;
                    frmYesterday.label7.Text = "IR" + yester2;
                    frmYesterday.label10.Text = "IR" + yester3;
                    frmYesterday.label13.Text = "IR" + yester4;
                    frmYesterday.label5.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label6.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label9.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label12.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label15.Text = "IRANIAN RIAL";
                }
                catch (Exception)
                {

                    frmYesterday.label1.Visible = true;
                    frmYesterday.label2.Visible = true;
                    frmYesterday.pictureBox1.Visible = true;
                    frmYesterday.label4.Text = "IR0";
                    frmYesterday.label7.Text = "IR0";
                    frmYesterday.label10.Text = "IR0";
                    frmYesterday.label13.Text = "IR0";
                }

            }
            else if (metroComboBox1.Text == "CNY")
            {
                string TodayData = "https://www.tcmb.gov.tr/kurlar/today.xml";
                var xmldoc = new XmlDocument();
                xmldoc.Load(TodayData);
                DateTime date = Convert.ToDateTime(xmldoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='CNY']/ForexBuying").InnerXml;
                string usd2 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='CNY']/ForexSelling").InnerXml;
                string usd3 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='CNY']/BanknoteBuying").InnerXml;
                string usd4 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='CNY']/BanknoteSelling").InnerXml;
                frmdashboard.label4.Text = "¥" + usd;
                frmdashboard.label7.Text = "¥" + usd2;
                frmdashboard.label10.Text = "¥" + usd3;
                frmdashboard.label13.Text = "¥" + usd4;
                frmdashboard.label5.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label6.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label9.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label12.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label15.Text = "CHINESE RENMINBI";

                Thread.Sleep(1000);

                //-------------------------------------------------------------------
                //YESTERDAY DATA
                //degiskenlere tarihi atıyoruz
                try
                {
                    int years, month, day;
                    day = DateTime.Now.Day;
                    month = DateTime.Now.Month;
                    years = DateTime.Now.Year;
                    string url;
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (month < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (month < 10 && day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    //dunku kur verilerini almak icin url degisimi - bugun -1


                    //degisen urlyi yazıyoruz
                    string yesterData = url;
                    var xmldocY = new XmlDocument();
                    xmldocY.Load(yesterData);
                    DateTime date2 = Convert.ToDateTime(xmldocY.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                    string yester1 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='CNY']/ForexBuying").InnerXml;
                    string yester2 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='CNY']/ForexSelling").InnerXml;
                    string yester3 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='CNY']/BanknoteBuying").InnerXml;
                    string yester4 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='CNY']/BanknoteSelling").InnerXml;
                    frmYesterday.label4.Text = "¥" + yester1;
                    frmYesterday.label7.Text = "¥" + yester2;
                    frmYesterday.label10.Text = "¥" + yester3;
                    frmYesterday.label13.Text = "¥" + yester4;
                    frmYesterday.label5.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label6.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label9.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label12.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label15.Text = "CHINESE RENMINBI";
                }
                catch (Exception)
                {

                    frmYesterday.label1.Visible = true;
                    frmYesterday.label2.Visible = true;
                    frmYesterday.pictureBox1.Visible = true;
                    frmYesterday.label4.Text = "¥0";
                    frmYesterday.label7.Text = "¥0";
                    frmYesterday.label10.Text = "¥0";
                    frmYesterday.label13.Text = "¥0";
                }

            }
            else if (metroComboBox1.Text == "PKR")
            {
                string TodayData = "https://www.tcmb.gov.tr/kurlar/today.xml";
                var xmldoc = new XmlDocument();
                xmldoc.Load(TodayData);
                DateTime date = Convert.ToDateTime(xmldoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='PKR']/ForexBuying").InnerXml;
                string usd2 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='PKR']/ForexSelling").InnerXml;
                string usd3 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='PKR']/BanknoteBuying").InnerXml;
                string usd4 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='PKR']/BanknoteSelling").InnerXml;
                frmdashboard.label4.Text = "PK" + usd;
                frmdashboard.label7.Text = "PK" + usd2;
                frmdashboard.label10.Text = "PK" + usd3;
                frmdashboard.label13.Text = "PK" + usd4;
                frmdashboard.label5.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label6.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label9.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label12.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label15.Text = "PAKISTANI RUPEE";

                Thread.Sleep(1000);

                //-------------------------------------------------------------------
                //YESTERDAY DATA
                //degiskenlere tarihi atıyoruz
                try
                {
                    int years, month, day;
                    day = DateTime.Now.Day;
                    month = DateTime.Now.Month;
                    years = DateTime.Now.Year;
                    string url;
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (month < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (month < 10 && day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    //dunku kur verilerini almak icin url degisimi - bugun -1


                    //degisen urlyi yazıyoruz
                    string yesterData = url;
                    var xmldocY = new XmlDocument();
                    xmldocY.Load(yesterData);
                    DateTime date2 = Convert.ToDateTime(xmldocY.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                    string yester1 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='PKR']/ForexBuying").InnerXml;
                    string yester2 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='PKR']/ForexSelling").InnerXml;
                    string yester3 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='PKR']/BanknoteBuying").InnerXml;
                    string yester4 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='PKR']/BanknoteSelling").InnerXml;
                    frmYesterday.label4.Text = "PK" + yester1;
                    frmYesterday.label7.Text = "PK" + yester2;
                    frmYesterday.label10.Text = "PK" + yester3;
                    frmYesterday.label13.Text = "PK" + yester4;
                    frmYesterday.label5.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label6.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label9.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label12.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label15.Text = "PAKISTANI RUPEE";
                }
                catch (Exception)
                {

                    frmYesterday.label1.Visible = true;
                    frmYesterday.label2.Visible = true;
                    frmYesterday.pictureBox1.Visible = true;
                    frmYesterday.label4.Text = "PK0";
                    frmYesterday.label7.Text = "PK0";
                    frmYesterday.label10.Text = "PK0";
                    frmYesterday.label13.Text = "PK0";
                }

            }
            else if (metroComboBox1.Text == "QAR")
            {
                string TodayData = "https://www.tcmb.gov.tr/kurlar/today.xml";
                var xmldoc = new XmlDocument();
                xmldoc.Load(TodayData);
                DateTime date = Convert.ToDateTime(xmldoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='QAR']/ForexBuying").InnerXml;
                string usd2 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='QAR']/ForexSelling").InnerXml;
                string usd3 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='QAR']/BanknoteBuying").InnerXml;
                string usd4 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='QAR']/BanknoteSelling").InnerXml;
                frmdashboard.label4.Text = "Q" + usd;
                frmdashboard.label7.Text = "Q" + usd2;
                frmdashboard.label10.Text = "Q" + usd3;
                frmdashboard.label13.Text = "Q" + usd4;
                frmdashboard.label5.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label6.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label9.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label12.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label15.Text = "QATARI RIAL";

                Thread.Sleep(1000);

                //-------------------------------------------------------------------
                //YESTERDAY DATA
                //degiskenlere tarihi atıyoruz
                try
                {
                    int years, month, day;
                    day = DateTime.Now.Day;
                    month = DateTime.Now.Month;
                    years = DateTime.Now.Year;
                    string url;
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (month < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (month < 10 && day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    //dunku kur verilerini almak icin url degisimi - bugun -1


                    //degisen urlyi yazıyoruz
                    string yesterData = url;
                    var xmldocY = new XmlDocument();
                    xmldocY.Load(yesterData);
                    DateTime date2 = Convert.ToDateTime(xmldocY.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                    string yester1 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='QAR']/ForexBuying").InnerXml;
                    string yester2 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='QAR']/ForexSelling").InnerXml;
                    string yester3 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='QAR']/BanknoteBuying").InnerXml;
                    string yester4 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='QAR']/BanknoteSelling").InnerXml;
                    frmYesterday.label4.Text = "Q" + yester1;
                    frmYesterday.label7.Text = "Q" + yester2;
                    frmYesterday.label10.Text = "Q" + yester3;
                    frmYesterday.label13.Text = "Q" + yester4;
                    frmYesterday.label5.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label6.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label9.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label12.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label15.Text = "QATARI RIAL";
                }
                catch (Exception)
                {

                    frmYesterday.label1.Visible = true;
                    frmYesterday.label2.Visible = true;
                    frmYesterday.pictureBox1.Visible = true;
                    frmYesterday.label4.Text = "Q0";
                    frmYesterday.label7.Text = "Q0";
                    frmYesterday.label10.Text = "Q0";
                    frmYesterday.label13.Text = "Q0";
                }

            }
            else if (metroComboBox1.Text == "KRW")
            {
                string TodayData = "https://www.tcmb.gov.tr/kurlar/today.xml";
                var xmldoc = new XmlDocument();
                xmldoc.Load(TodayData);
                DateTime date = Convert.ToDateTime(xmldoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='KRW']/ForexBuying").InnerXml;
                string usd2 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='KRW']/ForexSelling").InnerXml;
                string usd3 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='KRW']/BanknoteBuying").InnerXml;
                string usd4 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='KRW']/BanknoteSelling").InnerXml;
                frmdashboard.label4.Text = "₩" + usd;
                frmdashboard.label7.Text = "₩" + usd2;
                frmdashboard.label10.Text = "₩" + usd3;
                frmdashboard.label13.Text = "₩" + usd4;
                frmdashboard.label5.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label6.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label9.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label12.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label15.Text = "SOUTH KOREAN WON";

                Thread.Sleep(1000);

                //-------------------------------------------------------------------
                //YESTERDAY DATA
                //degiskenlere tarihi atıyoruz
                try
                {
                    int years, month, day;
                    day = DateTime.Now.Day;
                    month = DateTime.Now.Month;
                    years = DateTime.Now.Year;
                    string url;
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (month < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (month < 10 && day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    //dunku kur verilerini almak icin url degisimi - bugun -1


                    //degisen urlyi yazıyoruz
                    string yesterData = url;
                    var xmldocY = new XmlDocument();
                    xmldocY.Load(yesterData);
                    DateTime date2 = Convert.ToDateTime(xmldocY.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                    string yester1 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='KRW']/ForexBuying").InnerXml;
                    string yester2 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='KRW']/ForexSelling").InnerXml;
                    string yester3 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='KRW']/BanknoteBuying").InnerXml;
                    string yester4 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='KRW']/BanknoteSelling").InnerXml;
                    frmYesterday.label4.Text = "₩" + yester1;
                    frmYesterday.label7.Text = "₩" + yester2;
                    frmYesterday.label10.Text = "₩" + yester3;
                    frmYesterday.label13.Text = "₩" + yester4;
                    frmYesterday.label5.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label6.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label9.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label12.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label15.Text = "SOUTH KOREAN WON";
                }
                catch (Exception)
                {

                    frmYesterday.label1.Visible = true;
                    frmYesterday.label2.Visible = true;
                    frmYesterday.pictureBox1.Visible = true;
                    frmYesterday.label4.Text = "₩0";
                    frmYesterday.label7.Text = "₩0";
                    frmYesterday.label10.Text = "₩0";
                    frmYesterday.label13.Text = "₩0";
                }

            }
            else if (metroComboBox1.Text == "AZN")
            {
                string TodayData = "https://www.tcmb.gov.tr/kurlar/today.xml";
                var xmldoc = new XmlDocument();
                xmldoc.Load(TodayData);
                DateTime date = Convert.ToDateTime(xmldoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='AZN']/ForexBuying").InnerXml;
                string usd2 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='AZN']/ForexSelling").InnerXml;
                string usd3 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='AZN']/BanknoteBuying").InnerXml;
                string usd4 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='AZN']/BanknoteSelling").InnerXml;
                frmdashboard.label4.Text = "AZ" + usd;
                frmdashboard.label7.Text = "AZ" + usd2;
                frmdashboard.label10.Text = "AZ" + usd3;
                frmdashboard.label13.Text = "AZ" + usd4;
                frmdashboard.label5.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label6.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label9.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label12.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label15.Text = "AZERBAIJANI NEW MANAT";

                Thread.Sleep(1000);

                //-------------------------------------------------------------------
                //YESTERDAY DATA
                //degiskenlere tarihi atıyoruz
                try
                {
                    int years, month, day;
                    day = DateTime.Now.Day;
                    month = DateTime.Now.Month;
                    years = DateTime.Now.Year;
                    string url;
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (month < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (month < 10 && day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    //dunku kur verilerini almak icin url degisimi - bugun -1


                    //degisen urlyi yazıyoruz
                    string yesterData = url;
                    var xmldocY = new XmlDocument();
                    xmldocY.Load(yesterData);
                    DateTime date2 = Convert.ToDateTime(xmldocY.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                    string yester1 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='AZN']/ForexBuying").InnerXml;
                    string yester2 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='AZN']/ForexSelling").InnerXml;
                    string yester3 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='AZN']/BanknoteBuying").InnerXml;
                    string yester4 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='AZN']/BanknoteSelling").InnerXml;
                    frmYesterday.label4.Text = "AZ" + yester1;
                    frmYesterday.label7.Text = "AZ" + yester2;
                    frmYesterday.label10.Text = "AZ" + yester3;
                    frmYesterday.label13.Text = "AZ" + yester4;
                    frmYesterday.label5.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label6.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label9.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label12.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label15.Text = "AZERBAIJANI NEW MANAT";
                }
                catch (Exception)
                {

                    frmYesterday.label1.Visible = true;
                    frmYesterday.label2.Visible = true;
                    frmYesterday.pictureBox1.Visible = true;
                    frmYesterday.label4.Text = "AZ0";
                    frmYesterday.label7.Text = "AZ0";
                    frmYesterday.label10.Text = "AZ0";
                    frmYesterday.label13.Text = "AZ0";
                }

            }
            else if (metroComboBox1.Text == "AED")
            {
                string TodayData = "https://www.tcmb.gov.tr/kurlar/today.xml";
                var xmldoc = new XmlDocument();
                xmldoc.Load(TodayData);
                DateTime date = Convert.ToDateTime(xmldoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='AED']/ForexBuying").InnerXml;
                string usd2 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='AED']/ForexSelling").InnerXml;
                string usd3 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='AED']/BanknoteBuying").InnerXml;
                string usd4 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='AED']/BanknoteSelling").InnerXml;
                frmdashboard.label4.Text = "AE" + usd;
                frmdashboard.label7.Text = "AE" + usd2;
                frmdashboard.label10.Text = "AE" + usd3;
                frmdashboard.label13.Text = "AE" + usd4;
                frmdashboard.label5.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label6.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label9.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label12.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label15.Text = "UNITED ARAB EMIRATES DIRHAM";

                Thread.Sleep(1000);

                //-------------------------------------------------------------------
                //YESTERDAY DATA
                //degiskenlere tarihi atıyoruz
                try
                {
                    int years, month, day;
                    day = DateTime.Now.Day;
                    month = DateTime.Now.Month;
                    years = DateTime.Now.Year;
                    string url;
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (month < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (month < 10 && day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    //dunku kur verilerini almak icin url degisimi - bugun -1


                    //degisen urlyi yazıyoruz
                    string yesterData = url;
                    var xmldocY = new XmlDocument();
                    xmldocY.Load(yesterData);
                    DateTime date2 = Convert.ToDateTime(xmldocY.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                    string yester1 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='AED']/ForexBuying").InnerXml;
                    string yester2 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='AED']/ForexSelling").InnerXml;
                    string yester3 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='AED']/BanknoteBuying").InnerXml;
                    string yester4 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='AED']/BanknoteSelling").InnerXml;
                    frmYesterday.label4.Text = "AE" + yester1;
                    frmYesterday.label7.Text = "AE" + yester2;
                    frmYesterday.label10.Text = "AE" + yester3;
                    frmYesterday.label13.Text = "AE" + yester4;
                    frmYesterday.label5.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label6.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label9.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label12.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label15.Text = "UNITED ARAB EMIRATES DIRHAM";
                }
                catch (Exception)
                {

                    frmYesterday.label1.Visible = true;
                    frmYesterday.label2.Visible = true;
                    frmYesterday.pictureBox1.Visible = true;
                    frmYesterday.label4.Text = "AE0";
                    frmYesterday.label7.Text = "AE0";
                    frmYesterday.label10.Text = "AE0";
                    frmYesterday.label13.Text = "AE0";
                }

            }
            else if (metroComboBox1.Text == "XDR")
            {
                string TodayData = "https://www.tcmb.gov.tr/kurlar/today.xml";
                var xmldoc = new XmlDocument();
                xmldoc.Load(TodayData);
                DateTime date = Convert.ToDateTime(xmldoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='XDR']/ForexBuying").InnerXml;
                string usd2 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='XDR']/ForexSelling").InnerXml;
                string usd3 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='XDR']/BanknoteBuying").InnerXml;
                string usd4 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='XDR']/BanknoteSelling").InnerXml;
                frmdashboard.label4.Text = "XD" + usd;
                frmdashboard.label7.Text = "XD" + usd2;
                frmdashboard.label10.Text = "XD" + usd3;
                frmdashboard.label13.Text = "XD" + usd4;
                frmdashboard.label5.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label6.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label9.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label12.Text = string.Format("{0}", date.ToShortDateString());
                frmdashboard.label15.Text = "SPECIAL DRAWING RIGHT (SDR)";

                Thread.Sleep(1000);

                //-------------------------------------------------------------------
                //YESTERDAY DATA
                //degiskenlere tarihi atıyoruz
                try
                {
                    int years, month, day;
                    day = DateTime.Now.Day;
                    month = DateTime.Now.Month;
                    years = DateTime.Now.Year;
                    string url;
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (month < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    if (month < 10 && day < 10)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                     Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) + "0" +
                     Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                    //dunku kur verilerini almak icin url degisimi - bugun -1


                    //degisen urlyi yazıyoruz
                    string yesterData = url;
                    var xmldocY = new XmlDocument();
                    xmldocY.Load(yesterData);
                    DateTime date2 = Convert.ToDateTime(xmldocY.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                    string yester1 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='XDR']/ForexBuying").InnerXml;
                    string yester2 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='XDR']/ForexSelling").InnerXml;
                    string yester3 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='XDR']/BanknoteBuying").InnerXml;
                    string yester4 = xmldocY.SelectSingleNode("Tarih_Date/Currency [@Kod='XDR']/BanknoteSelling").InnerXml;
                    frmYesterday.label4.Text = "XD" + yester1;
                    frmYesterday.label7.Text = "XD" + yester2;
                    frmYesterday.label10.Text = "XD" + yester3;
                    frmYesterday.label13.Text = "XD" + yester4;
                    frmYesterday.label5.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label6.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label9.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label12.Text = string.Format("{0}", date2.ToShortDateString());
                    frmYesterday.label15.Text = "SPECIAL DRAWING RIGHT (SDR)";
                }
                catch (Exception)
                {

                    frmYesterday.label1.Visible = true;
                    frmYesterday.label2.Visible = true;
                    frmYesterday.pictureBox1.Visible = true;
                    frmYesterday.label4.Text = "XD0";
                    frmYesterday.label7.Text = "XD0";
                    frmYesterday.label10.Text = "XD0";
                    frmYesterday.label13.Text = "XD0";
                }
          
                
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }

        //----------------------------------------------
        //FORMU EKRANDA SURUKLEMEK ICIN ISLEMLER - FORM 
        private void exchange_MouseDown(object sender, MouseEventArgs e)
        {
            x = e.X;
            y = e.Y;
            location = true;
        }

        private void exchange_MouseUp(object sender, MouseEventArgs e)
        {
            location = false;
        }

        private void exchange_MouseMove(object sender, MouseEventArgs e)
        {
            if (location == true)
            {
                this.Location = new Point(Cursor.Position.X - x, Cursor.Position.Y - y);
            }
        }

        //------------------------------------------------------
        //FORMU EKRANDA SURUKLEMEK ICIN ISLEMLER - ORTADAKI PANEL
        private void pnlLoader_MouseDown(object sender, MouseEventArgs e)
        {
            x = e.X;
            y = e.Y;
            location = true;
        }

        private void pnlLoader_MouseUp(object sender, MouseEventArgs e)
        {
            location = false;

        }

        private void pnlLoader_MouseMove(object sender, MouseEventArgs e)
        {
            if (location == true)
            {
                this.Location = new Point(Cursor.Position.X - x, Cursor.Position.Y - y);
            }
        }
        //FORMU EKRANDA SURUKLEMEK İCİN İSLEMLER - LOGO
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            x = e.X;
            y = e.Y;
            location = true;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            location = false;

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (location == true)
            {
                this.Location = new Point(Cursor.Position.X - x, Cursor.Position.Y - y);
            }
        }
        //FORMU EKRANDA SURUKLEMEK ICIN ISLEMNLER - LOGO PANEL KISMI ICIN
        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            x = e.X;
            y = e.Y;
            location = true;
        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            location = false;

        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (location == true)
            {
                this.Location = new Point(Cursor.Position.X - x, Cursor.Position.Y - y);
            }
        }
    }
}
