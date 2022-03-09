using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using HtmlAgilityPack;
using System.Threading;

namespace Kur_Bilgisi
{
    public partial class analysis : Form
    {
        public double buying1, buying2, buying3;
        public double buying4, buying5, buying6;

        public double selling1, selling2, selling3;
        public double selling4, selling5, selling6;

        public double effective_buying1,effective_buying2,effective_buying3;
        public double effective_buying4,effective_buying5,effective_buying6;


        public analysis()
        {
            InitializeComponent();
            System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false;
            Thread.Sleep(5000);
            Thread A1 = new Thread(new ThreadStart(Analysis_data1));
            A1.Start();
            Thread.Sleep(3000);
            Thread A2 = new Thread(new ThreadStart(Analysis_data2));
            A2.Start();
            Thread.Sleep(3000);
            Thread A3 = new Thread(new ThreadStart(Analysis_data3));
            A3.Start();
            Thread.Sleep(3000);
            Thread A4 = new Thread(new ThreadStart(Analysis_data4));
            A4.Start();
            Thread.Sleep(3000);
            Thread A5 = new Thread(new ThreadStart(Analysis_data5));
            A5.Start();
            Thread.Sleep(3000);
            Thread TData = new Thread(new ThreadStart(Today_data));
            TData.Start();
            Thread.Sleep(2000);
            graphic();
        }

        public void Today_data()
        {
            Thread.Sleep(1);
            string TodayData = "https://www.tcmb.gov.tr/kurlar/today.xml";
            var xmldoc = new XmlDocument();
            xmldoc.Load(TodayData);
            DateTime date = Convert.ToDateTime(xmldoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
            string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/ForexBuying").InnerXml;
            string usd2 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/ForexSelling").InnerXml;
            string usd3 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/BanknoteBuying").InnerXml;
            string usd4 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/BanknoteSelling").InnerXml;
            buying6 = Convert.ToDouble(usd);
            selling6 = Convert.ToDouble(usd2);
            effective_buying6 = Convert.ToDouble(usd3);
            today.Text = buying6.ToString();
        }


        public void Analysis_data1()
        {
            try
            {
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
                    if (day == 1)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                       Convert.ToString(month) + "/" + "30" +
                       Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }

                }
                if (month < 10 && day < 10)
                {
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                 Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 1) + "0" +
                 Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (day == 1)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                      Convert.ToString(month) + "/" + "30" + "0" +
                      Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                }

                //dunku kur verilerini almak icin url degisimi - bugun -1
                //degisen urlyi yazıyoruz
                string TodayData = url;
                var xmldoc = new XmlDocument();
                xmldoc.Load(TodayData);
                DateTime date = Convert.ToDateTime(xmldoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value); //tarih
                string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/ForexBuying").InnerXml;//satis
                string usd2 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/ForexSelling").InnerXml;//alis
                string usd3 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/BanknoteBuying").InnerXml;//efektif satis
                string usd4 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/BanknoteSelling").InnerXml;//efektif alis
                buying1 = Convert.ToDouble(usd);
                selling1 = Convert.ToDouble(usd2);
                effective_buying1 = Convert.ToDouble(usd3);
                YesterLbl.Text = buying1.ToString();
                label1.Visible = false;
            }
            catch (Exception)
            {
                label1.Visible = true;
              
            }
     
        }

        public void Analysis_data2()
        {

            try
            {
                //degiskenlere tarihi atıyoruz
                int years, month, day;
                day = DateTime.Now.Day;
                month = DateTime.Now.Month;
                years = DateTime.Now.Year;
                string url;
                url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                 Convert.ToString(month) + "/" + Convert.ToString(day - 2) +
                 Convert.ToString(month) + Convert.ToString(years) + ".xml";
                if (month < 10)
                {
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                 Convert.ToString(month) + "/" + Convert.ToString(day - 2) + "0" +
                 Convert.ToString(month) + Convert.ToString(years) + ".xml";
                }
                if (day < 10)
                {
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                 Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 2) +
                 Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (day == 1)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                       Convert.ToString(month) + "/" + "29" +
                       Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }

                }
                if (month < 10 && day < 10)
                {
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                 Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 2) + "0" +
                 Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (day == 1)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                      Convert.ToString(month) + "/" + "29" + "0" +
                      Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                }

                //dunku kur verilerini almak icin url degisimi - bugun -1
                //degisen urlyi yazıyoruz
                string TodayData = url;
                var xmldoc = new XmlDocument();
                xmldoc.Load(TodayData);
                DateTime date = Convert.ToDateTime(xmldoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value); //tarih
                string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/ForexBuying").InnerXml;//satis
                string usd2 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/ForexSelling").InnerXml;//alis
                string usd3 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/BanknoteBuying").InnerXml;//efektif satis
                string usd4 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/BanknoteSelling").InnerXml;//efektif alis

                buying2 = Convert.ToDouble(usd);
                selling2 = Convert.ToDouble(usd2);
                effective_buying2 = Convert.ToDouble(usd3);
                TwoLbl.Text = buying2.ToString();
            }
            catch (Exception)
            {

                label1.Visible = true;
            }
        }

        public void Analysis_data3()
        {

            try
            {
                //degiskenlere tarihi atıyoruz
                int years, month, day;
                day = DateTime.Now.Day;
                month = DateTime.Now.Month;
                years = DateTime.Now.Year;
                string url;
                url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                 Convert.ToString(month) + "/" + Convert.ToString(day - 3) +
                 Convert.ToString(month) + Convert.ToString(years) + ".xml";
                if (month < 10)
                {
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                 Convert.ToString(month) + "/" + Convert.ToString(day - 3) + "0" +
                 Convert.ToString(month) + Convert.ToString(years) + ".xml";
                }
                if (day < 10)
                {
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                 Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 3) +
                 Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (day == 1)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                       Convert.ToString(month) + "/" + "28" +
                       Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }

                }
                if (month < 10 && day < 10)
                {
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                 Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 3) + "0" +
                 Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (day == 1)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                      Convert.ToString(month) + "/" + "28" + "0" +
                      Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                }

                //dunku kur verilerini almak icin url degisimi - bugun -1
                //degisen urlyi yazıyoruz
                string TodayData = url;
                var xmldoc = new XmlDocument();
                xmldoc.Load(TodayData);
                DateTime date = Convert.ToDateTime(xmldoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value); //tarih
                string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/ForexBuying").InnerXml;//satis
                string usd2 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/ForexSelling").InnerXml;//alis
                string usd3 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/BanknoteBuying").InnerXml;//efektif satis
                string usd4 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/BanknoteSelling").InnerXml;//efektif alis
                buying3 = Convert.ToDouble(usd);
                selling3 = Convert.ToDouble(usd2);
                effective_buying3 = Convert.ToDouble(usd3);
                ThreeLbl.Text = buying3.ToString();
            }
            catch (Exception)
            {

                label1.Visible = true;
            }
        }

        public void Analysis_data4()
        {

            try
            {
                //degiskenlere tarihi atıyoruz
                int years, month, day;
                day = DateTime.Now.Day;
                month = DateTime.Now.Month;
                years = DateTime.Now.Year;
                string url;
                url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                 Convert.ToString(month) + "/" + Convert.ToString(day - 4) +
                 Convert.ToString(month) + Convert.ToString(years) + ".xml";
                if (month < 10)
                {
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                 Convert.ToString(month) + "/" + Convert.ToString(day - 4) + "0" +
                 Convert.ToString(month) + Convert.ToString(years) + ".xml";
                }
                if (day < 10)
                {
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                 Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 4) +
                 Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (day == 1)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                       Convert.ToString(month) + "/" + "27" +
                       Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }

                }
                if (month < 10 && day < 10)
                {
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                 Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 4) + "0" +
                 Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (day == 1)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                      Convert.ToString(month) + "/" + "27" + "0" +
                      Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                }

                //dunku kur verilerini almak icin url degisimi - bugun -1
                //degisen urlyi yazıyoruz
                string TodayData = url;
                var xmldoc = new XmlDocument();
                xmldoc.Load(TodayData);
                DateTime date = Convert.ToDateTime(xmldoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value); //tarih
                string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/ForexBuying").InnerXml;//satis
                string usd2 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/ForexSelling").InnerXml;//alis
                string usd3 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/BanknoteBuying").InnerXml;//efektif satis
                string usd4 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/BanknoteSelling").InnerXml;//efektif alis
                buying4 = Convert.ToDouble(usd);
                selling4 = Convert.ToDouble(usd2);
                effective_buying4 = Convert.ToDouble(usd3);
                fourLbl.Text = buying4.ToString();
            }
            catch (Exception)
            {

                label1.Visible = true;
            }

        }

        public void Analysis_data5()
        {

            try
            {
                //degiskenlere tarihi atıyoruz
                int years, month, day;
                day = DateTime.Now.Day;
                month = DateTime.Now.Month;
                years = DateTime.Now.Year;
                string url;
                url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                 Convert.ToString(month) + "/" + Convert.ToString(day - 5) +
                 Convert.ToString(month) + Convert.ToString(years) + ".xml";
                if (month < 10)
                {
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                 Convert.ToString(month) + "/" + Convert.ToString(day - 5) + "0" +
                 Convert.ToString(month) + Convert.ToString(years) + ".xml";
                }
                if (day < 10)
                {
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                 Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 5) +
                 Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (day == 1)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) +
                       Convert.ToString(month) + "/" + "26" +
                       Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }

                }
                if (month < 10 && day < 10)
                {
                    url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                 Convert.ToString(month) + "/" + "0" + Convert.ToString(day - 5) + "0" +
                 Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    if (day == 1)
                    {
                        url = "https://www.tcmb.gov.tr/kurlar/" + Convert.ToString(years) + "0" +
                      Convert.ToString(month) + "/" + "26" + "0" +
                      Convert.ToString(month) + Convert.ToString(years) + ".xml";
                    }
                }

                //dunku kur verilerini almak icin url degisimi - bugun -1
                //degisen urlyi yazıyoruz
                string TodayData = url;
                var xmldoc = new XmlDocument();
                xmldoc.Load(TodayData);
                DateTime date = Convert.ToDateTime(xmldoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value); //tarih
                string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/ForexBuying").InnerXml;//satis
                string usd2 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/ForexSelling").InnerXml;//alis
                string usd3 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/BanknoteBuying").InnerXml;//efektif satis
                string usd4 = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/BanknoteSelling").InnerXml;//efektif alis
                buying5 = Convert.ToDouble(usd);
                selling5 = Convert.ToDouble(usd2);
                effective_buying5 = Convert.ToDouble(usd3);
                FiveLbl.Text = buying5.ToString();
            }
            catch (Exception)
            {

                label1.Visible = true;
            }
        }



        //grafikleri cizdirir
        public void graphic()//euro ve dolar graifklerini ciziyor
        {
            Thread.Sleep(3000);

            cartesianChart1.Series.Add(new LineSeries
            {
                Values = new ChartValues<double> { buying1, buying2, buying3, buying4, buying5,buying6 },
                ScalesYAt = 0
            });
            cartesianChart1.Series.Add(new LineSeries
            {
                Values = new ChartValues<double> { selling1,selling2, selling3, selling4, selling5,buying6 },
                ScalesYAt = 1
            });
            cartesianChart1.Series.Add(new LineSeries
            {
                Values = new ChartValues<double> { effective_buying1, effective_buying2, effective_buying3, effective_buying4, effective_buying5,effective_buying6 },
                ScalesYAt = 2
            });
            //now we add the 3 axes

            cartesianChart1.AxisY.Add(new Axis
            {
                Foreground = System.Windows.Media.Brushes.DodgerBlue,
                Title = "BUYİNG"
            });
            cartesianChart1.AxisY.Add(new Axis
            {
                Foreground = System.Windows.Media.Brushes.IndianRed,
                Title = "SELLİNG",
                Position = AxisPosition.RightTop
            });
            cartesianChart1.AxisY.Add(new Axis
            {
                Foreground = System.Windows.Media.Brushes.DarkOliveGreen,
                Title = "EFFECTİVE BUYİNG",
                Position = AxisPosition.RightTop
            });
        }

        private void analysis_Load(object sender, EventArgs e)
        {
            
        }
    }
}
