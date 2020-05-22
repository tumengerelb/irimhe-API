using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using irimhe.Models;

namespace irimhe.Controllers
{

    public class TempController : Controller
    {
        [HttpGet]
        public string Index()
        {
            TempModel temp = new TempModel();

            return temp.PullDataPG();
        }
       
        [HttpPost]
        public string irimhe_temp(DateTime dateTime,string station)
        {
            TempModel temp = new TempModel();
            string ret = "";

            if (string.IsNullOrEmpty(station))
            {
                var ten= temp.DateTimeTotenDay(dateTime);
                ret = temp.PullDataPGDB(ten);
            }
            else
            {
                var ten = temp.DateTimeTotenDay(dateTime);
                ret = temp.PullDataPGDB_station(ten, station);
            }

            return ret;
        }
        /*
        [HttpPost]
        public string station( DateTime ognoo,string station)
        {
            TempModel temp = new TempModel();
            var ten = temp.DateTimeTotenDay(ognoo);
            return 
        }
        [HttpPost]
        public string check(string station2)
        {
            TempModel temp = new TempModel();
            
            return temp.PullData_station(station2);
        }*/
        
    }
}
