using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FourSquare.SharpSquare.Core;
using FourSquare.SharpSquare.Entities;
using Microsoft.Win32;

namespace FourSquareData
{
    public class conect4Sq
    {
        const string FakeClientId = "FakeClientid";
        static string FakeClientSecret = "FakeClientSecret";


        // Fields
        private SharpSquare sharpSquare;
        public string thisRequest { get; private set; }
        public static string urlSOA = "http://fsq.apphb.com/";
        public string urlAuth { get; private set; }
        // Methods
        public conect4Sq()
            : this(ConfigurationManager.AppSettings["clientId"], ConfigurationManager.AppSettings["clientSecret"])
        {

        }

        public conect4Sq(string clientId, string clientSecret,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
    {
        if(clientId == FakeClientId){
            throw new ArgumentException(string.Format("please modify {0} with FourSquare information in the file {1} , line {2} function {3}" , FakeClientId, sourceFilePath,sourceLineNumber, memberName));
        }
         if(clientSecret== FakeClientSecret){
            throw new ArgumentException(string.Format("please modify {0} with FourSquare information in the file {1} , line {2} function {3}" , FakeClientSecret, sourceFilePath,sourceLineNumber, memberName));
        }

        this.thisRequest = Guid.NewGuid().ToString("N");
        this.sharpSquare = new SharpSquare(clientId, clientSecret);
        this.urlAuth = this.sharpSquare.GetAuthenticateUrl(urlSOA + "home/redirect4sq/" + this.thisRequest);
    }
        public void AuthenticateToken()
        {
            var newUrl = urlSOA + "api/Values/ClientToken/" + this.thisRequest;
            var code = new WebClient().DownloadString(newUrl).Replace("\"", "");
            SetAccessCode(code);

        }
        public void SetAccessCode(string code)
        {
            string token = this.sharpSquare.GetAccessToken(urlSOA + "home/redirect4sq/", code);
        }
        public void AuthenticateNewWebBrowser()
        {
            string urlAuth = this.sharpSquare.GetAuthenticateUrl(urlSOA + "home/redirect4sq/" + this.thisRequest);
            Process p = new Process();
            p.StartInfo.FileName = getDefaultBrowser();
            p.StartInfo.Arguments = urlAuth;
            p.Start();
            Thread.Sleep(5000);
            AuthenticateToken();
        }
        bool authenticated = false;

        public void Authenticate2()
        {

            var th = new Thread(() =>
            {

                var clicker = new WebBrowser { ScriptErrorsSuppressed = true };


                clicker.Visible = true;
                clicker.DocumentCompleted += clicker_DocumentCompleted;
                clicker.Navigate(urlAuth);
                Application.Run();
                //while (clicker.ReadyState != WebBrowserReadyState.Complete)
                //{
                //    Application.DoEvents();
                //}
            });
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
            int nrSecondsToWait = 5 * 60;
            while (!authenticated && nrSecondsToWait > 0)
            {
                Thread.Sleep(5000);
                nrSecondsToWait -= 5;
                Console.WriteLine("waiting" + nrSecondsToWait);

            }
            if (!authenticated)
            {
                throw new ArgumentException("not connected to foursquare");
            }
            AuthenticateToken();

        }

        void clicker_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (authenticated)
                return;
            if (!e.Url.OriginalString.StartsWith(urlSOA))
            {

                var f = new WebCredentials(urlAuth, urlSOA);
                f.ShowDialog();
                authenticated = (f.DialogResult == DialogResult.OK);

            }
            else
            {
                authenticated = true;
            }
            Application.ExitThread();
        }
        private string getDefaultBrowser()
        {
            string browser = string.Empty;
            RegistryKey key = null;
            try
            {
                key = Registry.ClassesRoot.OpenSubKey(@"HTTP\shell\open\command", false);

                //trim off quotes
                browser = key.GetValue(null).ToString().ToLower().Replace("\"", "");
                if (!browser.EndsWith("exe"))
                {
                    //get rid of everything after the ".exe"
                    browser = browser.Substring(0, browser.LastIndexOf(".exe") + 4);
                }
            }
            finally
            {
                if (key != null) key.Close();
            }
            return browser;
        }

        public static DateTime FromUnixTime(long unixTime)
        {
            var dt = new DateTime(621355968000000000L, DateTimeKind.Utc);
            return dt + TimeSpan.FromSeconds(unixTime);
        }


        private static long ToUnixTime(DateTime date)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan ts = date.ToUniversalTime() - epoch;
            return Convert.ToInt64(ts.TotalSeconds);
        }
        public List<Checkin> CheckinsBetween(DateTime? afterTimestamp, DateTime? beforeTimestamp)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            if (beforeTimestamp.HasValue)
            {
                d.Add("beforeTimestamp", ToUnixTime(beforeTimestamp.Value).ToString());
            }
            if (afterTimestamp.HasValue)
            {
                d.Add("afterTimestamp", ToUnixTime(afterTimestamp.Value).ToString());
            }
            if (d.Count == 0)
            {
                d = null;
            }
            return this.sharpSquare.GetUserCheckins("self", d);
        }
        public List<Checkin> CheckinsToday()
        {
            var now = DateTime.Now.Date;
            return CheckinsBetween(now, DateTime.Now.Date.AddDays(1));
        }
        public List<Checkin> CheckinsYesterday()
        {
            var now = DateTime.Now.Date.AddDays(-1);
            return CheckinsBetween(now, DateTime.Now.Date.AddDays(1));
        }
        public List<VenueHistory> VenuesToday()
        {
            var now = DateTime.Now.Date;

            return VenuesBetween(now, DateTime.Now.Date.AddDays(1));
        }
        public List<VenueHistory> VenuesYesterday()
        {
            var now = DateTime.Now.Date.AddDays(-1);

            return VenuesBetween(now, DateTime.Now.Date.AddDays(1));
        }
        public List<VenueHistory> VenuesBetween(DateTime? afterTimestamp, DateTime? beforeTimestamp)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            if (beforeTimestamp.HasValue)
            {
                d.Add("beforeTimestamp", ToUnixTime(beforeTimestamp.Value).ToString());
            }
            if (afterTimestamp.HasValue)
            {
                d.Add("afterTimestamp", ToUnixTime(afterTimestamp.Value).ToString());
            }
            if (d.Count == 0)
            {
                d = null;
            }
            return this.sharpSquare.GetUserVenueHistory(d);
        }





    }
}
