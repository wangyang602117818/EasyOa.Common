using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EasyOa.Common
{
    public static class HttpRequestHelper
    {
        /// <summary>
        /// 发送post请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public static string Post(string url, string paras)
        {
            return Post(url, paras, null, null, null);
        }
        /// <summary>
        /// 发送post请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="paras">参数</param>
        /// <param name="headers">请求包头</param>
        /// <returns></returns>
        public static string Post(string url, string paras, Dictionary<string, string> headers)
        {
            return Post(url, paras, headers, null, null);
        }
        /// <summary>
        /// 发送post请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="paras">参数</param>
        /// <param name="headers">请求包头</param>
        /// <param name="contentType">请求内容格式</param>
        /// <param name="accept">接收内容格式</param>
        /// <returns></returns>
        public static string Post(string url, string paras, Dictionary<string, string> headers, RequestContentType? contentType, AcceptType? accept, HttpMethodType method = HttpMethodType.post)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = method.GetDescription();
            if (headers != null)
            {
                foreach (KeyValuePair<string, string> kv in headers)
                {
                    request.Headers.Add(kv.Key, kv.Value);
                }
            }
            if (contentType != null) request.ContentType = contentType.GetDescription();
            if (accept != null) request.Accept = accept.GetDescription();
            byte[] bs = Encoding.UTF8.GetBytes(paras);
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(bs, 0, bs.Length);
            }
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                LogHelper.WriteException(ex);
                return null;
            }
        }
        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public static string Get(string url, string paras, HttpMethodType method = HttpMethodType.get)
        {
            if (!string.IsNullOrEmpty(paras)) url = url + "?" + paras;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = method.GetDescription();
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                LogHelper.WriteException(ex);
                return null;
            }
        }
        /// <summary>
        /// http协议上传本地文件
        /// </summary>
        /// <param name="url">上传文件的url地址</param>
        /// <param name="paras">参数</param>
        /// <param name="fullFilePath">本文件路径</param>
        /// <returns></returns>
        public static string PostWithLocalFile(string url, string paras, string fullFilePath)
        {
            
        }
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
