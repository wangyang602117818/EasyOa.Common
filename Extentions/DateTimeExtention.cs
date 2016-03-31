using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyOa.Common
{
    public static class DateTimeExtention
    {
        public static string UTCTimeStamp(this DateTime dateTime)
        {
            return ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
        }

        public static string BeijingTimeStamp(this DateTime dateTime)
        {
            return ((DateTime.Now.Ticks - 621355968000000000) / 10000000).ToString();
        }
    }
}
