using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using irimhe.Models;

namespace irimhe.Controllers
{
    public class StationController : Controller
    {
        [HttpGet]
        public string Index()
        {
            StationModel stationModel = new StationModel();
            
            return stationModel.GetStation();
        }
    }
}
