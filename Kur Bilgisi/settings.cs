using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net.Mail;
using System.Net;
using Microsoft.Win32;
using System.IO;

namespace Kur_Bilgisi
{
    public partial class settings : Form
    {
        string ProgramAdi = "KurBilgisi";
        string Dosya;
        public settings()
        {
            InitializeComponent();
            System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false;
            //ilk para birimi ayarı icin, para birimlerini degiskene atadık
            string[] Items = { "USD","AUD","DKK","EUR","GBP","CHF","SEK","CAD","KWD","NOK","SAR","JPY","BGN","RON","RUB",
            "IRR","CNY","PKR","QAR","KRW","AZN","AED","XDR"};
            metroComboBox1.Items.AddRange(Items);//daha sonra combobox a ekledik
            Dosya = "FirstCoin.txt";
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                if (key.GetValue(ProgramAdi).ToString() == "\"" + Application.ExecutablePath + "\"")
                { // Eğer regeditte varsa, checkbox ı işaretle
                    checkBox1.Checked = true;
                }
            }
            catch
            {

            }
            //Dosya yoksa Oluştur
            if (!File.Exists(Dosya))
            {
                File.Create(Dosya);

            }
            Thread.Sleep(1000);
            //dosyayı okuma modunda açıyoruz
            FileStream fileStream = new FileStream(Dosya, FileMode.OpenOrCreate, FileAccess.Read);
            //dosyadan satır satır okuyup textBox içine yazıdırıyoruz
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string satir = reader.ReadLine();
                label8.Text = satir;
                reader.Close();
            }
            fileStream.Close();
        }

        //geri bildirim gonderme
        private void send_Click(object sender, EventArgs e)
        {
            label4.Text = "";//label kısmını temizle -- succes ve uyarı labeli
            if (textBox1.Text != "" && textBox2.Text != "")//tum alanlar doluysa mesaj gonder fonksiyonunu calistir
            {
                Thread succs = new Thread(new ThreadStart(succes));
                succs.Start();
                pictureBox1.Visible = true;
                Thread send = new Thread(new ThreadStart(Send_FeedBack));
                send.Start();
            }
            else//alanlar bos ise kullanıcıyı uyar
            {
                label6.Text = "write the information!";
            }

        }
        //loading animasyonları
        public void Send_FeedBack()
        {
            Thread.Sleep(5000);
            pictureBox1.Visible = false;
            TelegramSendMessage();
        }
        public void succes()//gonderildi bilgisi
        {
            Thread.Sleep(5000);
            label4.Text = "SUCCESFULY";
        }
        //Telegram token ve id numarası
        string apilToken = "2054371522:AAFkP4gpLHo3tcY2XH-qZ3PfqIe0BU7MQkc";
        string destID = "2077055690";
        //Telegrama mesaj gondermek icin fonksiyon
        public void TelegramSendMessage()
        {
            try //gondermeyi dene
            {
                string text = "Problem: " + metroComboBox2.Text + "\n" + "GMAİL: " + textBox1.Text + "\n" + "FULLNAME: " + textBox2.Text + "\n" + "FROM EXCHANGE";
                string urlString = $"https://api.telegram.org/bot{apilToken}/sendMessage?chat_id={destID}&text={text}";

                WebClient webclient = new WebClient();
                webclient.DownloadString(urlString);

            }
            catch (Exception)
            {
                //bir sorun olursa no signal yazdır
                label7.Text = "no signal!";
            }

        }

        //textbox tıklama kontrolu
        bool gmail = false;
        bool fullname = false;

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            //tıkladıgında 1 seferlik temizle
            if (gmail == false)
            {
                textBox1.Clear();
                gmail = true;
            }
        }

        private void textBox2_MouseDown(object sender, MouseEventArgs e)
        {
            //tıkladıgında 1 seferlik temizle
            if (fullname == false)
            {
                textBox2.Clear();
                fullname = true;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                //baslangicta calistir
                label5.Text = "on";
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                key.SetValue(ProgramAdi, "\"" + Application.ExecutablePath + "\"");
            }
            else
            {
                //baslangictan kaldır
                label5.Text = "off";
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                key.DeleteValue(ProgramAdi);

            }
        }

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Dosya yoksa Oluştur
            if (!File.Exists(Dosya))
            {
                File.Create(Dosya);

            }
            File.WriteAllText(Dosya, metroComboBox1.Text);
            label8.Visible = false;
        }

        private void label8_Click(object sender, EventArgs e)
        {
            label8.Visible = false;

        }
    }
}
