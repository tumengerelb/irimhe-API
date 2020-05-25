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
        [HttpPost]
        public string irimhe_temp_date(DateTime beginDateTime,DateTime endDateTime)
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
        public string multi_station([System.Web.Http.FromBody]string[] sindex, [System.Web.Http.FromBody]string[] date, [System.Web.Http.FromBody]string[] ttt_aver)
        {
            string ret = "";

            TempModel temp = new TempModel();
            
            if( sindex.Length != 0 )
            {
                ret = temp.multi_station(sindex);
            }
            if(date.Length !=0)
            {

            }
            if(ttt_aver.Length !=0)
            {
                
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
