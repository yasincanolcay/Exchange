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

namespace Kur_Bilgisi
{
    public partial class Yesterday : Form
    {
        public Yesterday()
        {
            InitializeComponent();
            System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false;
        }
        double average_total;
        int avg2;

        private void Yesterday_Load(object sender, EventArgs e)
        {
            Thread avg = new Thread(new ThreadStart(Average));
            avg.Start();
        }
        public void Average()
        {
            try
            {
                string buying, selling, effectivebuy, effectivesell;
                buying = label4.Text.Remove(0, 1);
                selling = label7.Text.Remove(0, 1);
                effectivebuy = label10.Text.Remove(0, 1);
                effectivesell = label13.Text.Remove(0, 1);
                average_total = Convert.ToDouble(buying + selling + effectivebuy + effectivesell);
                avg2 = Convert.ToInt32(average_total) / 4;

            }
            catch (Exception)
            {

                average_total = 0;
            }

            Thread.Sleep(1000);
            if (avg2 >= 1 && avg2 <= 15000)
            {
                metroProgressSpinner1.Value = avg2;
                label18.Text = avg2.ToString();

            }
            if (avg2 > 15000)
            {
                avg2 = 15000;
                metroProgressSpinner1.Value = avg2;
                label18.Text = avg2.ToString();

            }
            if (avg2 < 1)
            {
                avg2 = 0;
                metroProgressSpinner1.Value = avg2;
                label18.Text = avg2.ToString();

            }
        }

        private void label4_TextChanged(object sender, EventArgs e)
        {
            Thread avg = new Thread(new ThreadStart(Average));
            avg.Start();
        }
    }
}
