using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kur_Bilgisi
{
    public partial class about : Form
    {
        public about()
        {
            InitializeComponent();
        }

        private void about_Load(object sender, EventArgs e)
        {
                
            label7.Text = " Exchange rate application has been made with olcay software" +
                " and provides instant tracking of many currencies." +
                "If you get an error, please contact us. Your opinions and suggestions are " +
                "important to us. The exchange rate app is completely free to use.  ";
        
        }
    }
}
