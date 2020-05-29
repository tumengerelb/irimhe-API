using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using irimhe.Models;

namespace irimhe.Controllers
{

    public class CatalPController : Controller
    {

        [HttpGet]
        public string Index()
        {
            CatalPModel catal = new CatalPModel();

            //return catal.GetCatalP();
            return "Ok";
        }
        [HttpPost]
        public string catal_index(string station)
        {
            CatalPModel catal = new CatalPModel();

            int req = Int32.Parse(station);
            return "Ok";
            //return catal.GetCatalP_station(req);
        }
    }
}
