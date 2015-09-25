using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyOa.Common.Util
{
    /// <summary>
    /// 把对象变成用&符号连接的请求参数
    /// </summary>
    public static class BuildRequestParams
    {
        public static string BuildParas(NameValueCollection nv)
        {
            if (nv == null || nv.Count == 0) return "";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < nv.Keys.Count; i++)
            {
                sb.Append(nv.Keys[i] + "=" + nv[nv.Keys[i]] + "&");
            }
            return sb.ToString().TrimEnd('&');
        }
        public static string BuildParas(Hashtable ht)
        {
            if (ht == null || ht.Count == 0) return "";
            StringBuilder sb = new StringBuilder();
            foreach (string key in ht.Keys)
            {
                sb.Append(key + "=" + ht[key] + "&");
            }
            return sb.ToString().TrimEnd('&');
        }
        public static string BuildParas(Dictionary<string, string> dict)
        {
            if (dict == null || dict.Count == 0) return "";
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in dict)
            {
                sb.Append(kv.Key + "=" + kv.Value + "&");
            }
            return sb.ToString().TrimEnd('&');
        }
    }
}
