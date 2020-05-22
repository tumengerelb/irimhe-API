using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using irimhe.Models;

namespace irimhe.Controllers
{

    public class SnowController:Controller
    {
        [HttpGet]
        public string Index()
        {
            SnowModel snow = new SnowModel();
            return snow.PullDataPG();
        }
        [HttpPost]
        public string irimhe_snow(DateTime dateTime,string station)
        {
            SnowModel temp = new SnowModel();
            string ret = "";

            if (string.IsNullOrEmpty(station))
            {
                var ten = temp.DateTimeTotenDay(dateTime);
                ret = temp.PullDataPGDB_station(ten);
            }
            else
            {
                var ten = temp.DateTimeTotenDay(dateTime);
                ret = temp.PullData_station(station,ten);
            }

            return ret;
        }
    }
}
