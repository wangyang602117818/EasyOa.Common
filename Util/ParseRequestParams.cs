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
        /// <summary>
        /// 参数字符串
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        public static NameValueCollection ParseToNameValueCollection(string requestParams)
        {
            NameValueCollection nv = new NameValueCollection();
            if (string.IsNullOrEmpty(requestParams)) return nv;
            string[] paramArray = requestParams.Split('&');
            foreach (string str in paramArray)
            {
                string[] item = str.Split('=');
                string key = item[0], value = item[1];
                nv.Add(key, value);
            }
            return nv;
        }

    }
}
