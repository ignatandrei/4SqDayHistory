using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FourSquare.SharpSquare.Core;
using FourSquare.SharpSquare.Entities;

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
        string urlAuth = this.sharpSquare.GetAuthenticateUrl(this.url + "home/redirect4Sq/" + this.thisRequest);
        Process p = new Process();
        p.StartInfo.FileName = urlAuth;
        p.Start();
        p.WaitForExit();
        Task<string> q = new WebClient().DownloadStringTaskAsync(this.url + "Values/ClientToken/" + this.thisRequest);
        string token = this.sharpSquare.GetAccessToken("url", q.Result);
    }

    private static long ToUnixTime(DateTime date)
    {
        DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        TimeSpan ts = date.ToUniversalTime() - epoch;
        return Convert.ToInt64(ts.TotalSeconds);
    }
    public List<VenueHistory> VenuesToday()
    {
        var now=DateTime.Now.Date;

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
