using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using irimhe.Models;

namespace irimhe.Controllers
{
    public class CatalTController : Controller
    {
        
        [HttpGet]
        public string Index()
        {
            CatalTModel catal = new CatalTModel();

            return catal.GetCatalT();
        }
        [HttpPost]
        public string catal_index(string station)
        {
            CatalTModel catal = new CatalTModel();

            int req = Int32.Parse(station);
            
            return catal.GetCatal_station(req);
        }
    }
}
