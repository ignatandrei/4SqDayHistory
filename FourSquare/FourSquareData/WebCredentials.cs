using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FourSquareData
{
    public partial class WebCredentials : Form
    {
        public WebCredentials()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.None;
        }
        string finalUrl; 
        public WebCredentials(string url, string finalUrl):this()
        {
            this.finalUrl = finalUrl;
            this.wb4Sq.Navigate(url);
        }

        private void wb4Sq_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (e.Url.ToString().StartsWith(this.finalUrl))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        
    }
}
