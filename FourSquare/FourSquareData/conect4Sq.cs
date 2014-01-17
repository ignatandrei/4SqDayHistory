using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FourSquare.SharpSquare.Core;
using FourSquare.SharpSquare.Entities;
using Microsoft.Win32;

namespace FourSquareData
{
   public class conect4Sq
{
    // Fields
    private SharpSquare sharpSquare;
    private string thisRequest = Guid.NewGuid().ToString("N");
    private string url = "http://fsq.apphb.com/";

    // Methods
    public conect4Sq(string clientId, string clientSecret)
    {
        this.sharpSquare = new SharpSquare(clientId, clientSecret);
    }

    public void Authenticate()
    {
        string urlAuth = this.sharpSquare.GetAuthenticateUrl(this.url + "home/redirect4sq/" + this.thisRequest);
        Process p = new Process();
        p.StartInfo.FileName = getDefaultBrowser();
        p.StartInfo.Arguments = urlAuth;
        p.Start();
        Thread.Sleep(5000);
        var newUrl = this.url + "api/Values/ClientToken/" + this.thisRequest;
        Task<string> q = new WebClient().DownloadStringTaskAsync(newUrl);
        var code = q.Result.Replace("\"","");
        string token = this.sharpSquare.GetAccessToken(this.url + "home/redirect4sq/", code);
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
        return this.sharpSquare.GetUserCheckins("self",d);
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
        var now=DateTime.Now.Date;

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
