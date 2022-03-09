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
using System.Threading;

namespace Kur_Bilgisi
{
    public partial class Loading : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]//form kenarlarını yuvarlak yapmak icin dll
        /* 
         form border ozelliklerini degistirmek icin kodlar
         */
        private static extern IntPtr CreateRoundRectRgn(int NLeftRect
       , int NRightRect, int NTopRect, int NBottomRect, int NWidthEllipse, int NHeightEllipse);
        public Loading()
        {
            InitializeComponent();
            System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false;//thread islemlerini kullanmak icin kod
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));//form border radius ayarı
            circularProgressBar1.Value = 0;//prognes bar value sıfırla
        }

        private void Loading_Load(object sender, EventArgs e)
        {
            InternetKontrol();
        }
        private static string v = "Loading...";//loading labeli icin
        //internet baglantısını kontrol et
        public static bool InternetKontrol()
        {
            try
            {
                System.Net.Sockets.TcpClient kontrol_client = new System.Net.Sockets.TcpClient("www.google.com.tr", 80);
                kontrol_client.Close();
                return true;
            }
            catch
            {
                v = "No Internet!";
                return false;
            }
        }
        bool kontrol = InternetKontrol();//internet kontrolden donen sonucu al ve kontrole esitle
        private void timer1_Tick(object sender, EventArgs e)//timer hep calisir
        {
            if (circularProgressBar1.Value < 100 && kontrol == true)//internet varsa ve prognes bar 100debn kucukse value arttır
            {
                circularProgressBar1.Value += 1;

            }
            circularProgressBar1.Text = circularProgressBar1.Value.ToString() + "%";//value yanına yuzde isareti koy
            //value 100 ise ve internet varsa yani kontrol true ise programı ac
            if (circularProgressBar1.Value == 100 && kontrol == true)
            {
                timer1.Enabled = false;
                exchange dashboard = new exchange();
                dashboard.Show();
                this.Hide();
            }
            if (kontrol == false)//kontrol false ise internet yoktur, bu yüzden kullanıcıya bildir ve exit fonksiyonunu calistir
            {
                label2.Text = v;
                Thread exitApp = new Thread(new ThreadStart(exiting));
                exitApp.Start();
            }
        }
        public void exiting()//exit fonksiyonu, internet yoksa 5 saniye bekler,cıkacagını bildirir 3 saniye bekler ve kapatır
        {
            Thread.Sleep(5000);
            v = "exiting...";
            Thread.Sleep(3000);
            Application.Exit();
        }
  
    }
}
