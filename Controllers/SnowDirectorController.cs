using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using irimhe.Models;

namespace irimhe.Controllers
{
    public class SnowDirectorController : Controller
    {
        [HttpGet]
        public string Index()
        {
            SnowDirectionModel snow = new SnowDirectionModel();
            return snow.PullDataPG();
        }

        [HttpPost]
        public string blizzard(string beginDateTime,string endDateTime)
        {
            SnowDirectionModel snow = new SnowDirectionModel();
            string ret = "";

            var begindate = snow.DateTimeTenDay(beginDateTime);
            var enddate = snow.DateTimeTenDay(endDateTime);

            ret = snow.only_pull_snow(begindate, enddate);

            return ret;
        }
        [HttpPost]
        public string irimhe_snow(DateTime dateTime, string station)
        {
            SnowDirectionModel temp = new SnowDirectionModel();
            string ret = "";

            if (string.IsNullOrEmpty(station))
            {
                var ten = temp.DateTimeTotenDay(dateTime);
                ret = temp.PullDataPGDB(ten);
            }
            else
            {
                var ten = temp.DateTimeTotenDay(dateTime);
                ret = temp.PullDataPGDB_station(ten,station);
            }

            return ret;
        }

        [HttpPost]
        public string irimhe_snow_date(DateTime beginDateTime, DateTime endDateTime)
        {
            //
            SnowDirectionModel snow = new SnowDirectionModel();
            string ret = "";

            var begindate = snow.DateTimeTotenDay(beginDateTime);
            var enddate = snow.DateTimeTotenDay(endDateTime);

            ret = snow.PullData_begin_end(begindate, enddate);

            return ret;
        }

    }
}
