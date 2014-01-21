using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FourSquareData;

namespace FourSquareWeb.Controllers
{
    public class HomeController : Controller
    {
        public conect4Sq Conect
        {
            get
            {
                string name="conect4sq";
                var ses = this.HttpContext.Session;
                if (ses[name] == null)
                {
                    ses[name] = new conect4Sq(conect4Sq.FakeClientId,conect4Sq.FakeClientSecret);

                }
                return ses[name] as conect4Sq;

            }

            
        }

        public ActionResult Index()
        {

            //var c = Conect;
            ViewBag.Url4Sq = Conect.urlAuth;
            ViewBag.thisRequest = Conect.thisRequest;            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "FourSquare SOA Application";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Andrei Ignat";

            return View();
        }
        public ActionResult redirect4Sq(string id, string code)
        {
            System.Web.HttpContext.Current.Application[id] = code;
            return Content("Ok" + id + "->"+code);
        }
        public ActionResult ShowCheckins()
        {
            var conect = this.Conect;
            conect.AuthenticateToken();
            var data = conect.CheckinsYesterday();
            return View(data);
        }
    }
}