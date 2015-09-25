using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyOa.Common.Util
{
    /// <summary>
    /// 把用&符号连接的请求参数，变成各种对象
    /// </summary>
    public static class ParseRequestParams
    {
        public static NameValueCollection ParseToNameValueCollection(string requestDate)
        {
            if (string.IsNullOrEmpty(requestDate)) return null;

            return null;
        }
    }
}
