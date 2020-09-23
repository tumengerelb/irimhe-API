using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using irimhe.Models;
using Microsoft.SqlServer.Server;

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
        public string irimhe_temp(string dateTime,string station)
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
        [HttpGet]
        public string getDates()
        {
            TempModel temp = new TempModel();

            return temp.udur();
        }
        [HttpGet]
        public string Temperature(string ttt_aver, string ttt_min, string ttt_max, string beginDateTime, string endDateTime)
        {
            return Temperature2(ttt_aver, ttt_min, ttt_max, beginDateTime, endDateTime);
        }
        [HttpPost]
        public string Temperature2(string ttt_aver,string ttt_min,string ttt_max,string beginDateTime, string endDateTime)
        {
            TempModel temp = new TempModel();
            string ret = "";

            var begindate = temp.DateTimeTotenDay(beginDateTime);
            var enddate = temp.DateTimeTotenDay(endDateTime);

            ret = temp.only_pull_temp(ttt_aver,ttt_max,ttt_min,begindate, enddate);

            return ret;
        }
        [HttpGet]
        public string soil(string tx_aver, string tx_max, string tx_min, string beginDateTime, string endDateTime)
        {
            TempModel temp = new TempModel();

            string ret = "";

            var begindate = temp.DateTimeTotenDay(beginDateTime);
            var enddate = temp.DateTimeTotenDay(endDateTime);

            ret = temp.only_pull_soil(tx_aver,tx_max,tx_min,begindate, enddate);

            return ret;
        }
        [HttpGet]
        public string wind(string windmax,string beginDateTime, string endDateTime)
        {
            TempModel temp = new TempModel();

            string ret = "";

            var begindate = temp.DateTimeTotenDay(beginDateTime);
            var enddate = temp.DateTimeTotenDay(endDateTime);

            ret = temp.only_pull_wind(windmax, begindate, enddate);

            return ret;

        }
        [HttpGet]
        public string percipitation(string sum_rrr_max, string beginDateTime, string endDateTime)
        {
            TempModel temp = new TempModel();

            string ret = "";

            var begindate = temp.DateTimeTotenDay(beginDateTime);
            var enddate = temp.DateTimeTotenDay(endDateTime);

            ret = temp.only_pull_percipitation(sum_rrr_max, begindate, enddate);

            return ret;

        }
        [HttpPost]
        public string irimhe_temp_date(string beginDateTime,string endDateTime)
        {
            //
            TempModel temp = new TempModel();
            string ret = "";

            var begindate = temp.DateTimeTotenDay(beginDateTime);
            var enddate = temp.DateTimeTotenDay(endDateTime);

            ret = temp.PullData_begin_end(begindate,enddate);

            return ret;
        }
        
        [HttpPost]
        public string multi_station([System.Web.Http.FromBody]string[] sindex)
        {
            string ret = "";

            TempModel temp = new TempModel();
            
            if( sindex.Length != 0 )
            {
                ret = temp.multi_station(sindex);
            }
            else
            {
                ret = "error";
            }

            return ret;
        }
        /*
        [HttpPost]
        public string check(string station2)
        {
            TempModel temp = new TempModel();
            
            return temp.PullData_station(station2);
        }*/
        
    }
}
