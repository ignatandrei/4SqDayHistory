using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace FourSquareWeb.Controllers
{
    public class ValuesController : ApiController
    {
        public string GetData(string id)
        {
            return System.Web.HttpContext.Current.Application[id].ToString();
        }
    }
}
