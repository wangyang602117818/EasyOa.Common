using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EasyOa.Common.Util;

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
        public static string Post(string url, string paras, Dictionary<string, string> headers, RequestContentType? contentType, AcceptType? accept)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = HttpMethodType.Post.GetDescription();
            request.ContentType = RequestContentType.UrlEncoded.GetDescription();
            request.Accept = AcceptType.json.GetDescription();
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
                LogHelper.ErrorLog(ex);
                return null;
            }
        }
        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public static string Get(string url, string paras)
        {
            if (!string.IsNullOrEmpty(paras)) url = url + "?" + paras;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = HttpMethodType.Get.GetDescription();
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
                LogHelper.ErrorLog(ex);
                return null;
            }
        }
        public Task<string> PostFile(string url, Dictionary<string, byte[]> files, Dictionary<string, string> paras)
        {
            string boundary = "----Ij5ei4ae0ei4cH2ae0Ef1ei4Ij5gL6";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "post";
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            Stream stream = request.GetRequestStream();  //请求流
            foreach (var item in files)
            {
                //文件开始标记
                string fileBegin = "--" + boundary + "\r\nContent-Disposition: form-data;name=\"file\";filename=\"" + item.Key + "\"\r\nContent-Type: application/octet-stream; charset=utf-8\r\n\r\n";
                byte[] bytes = Encoding.UTF8.GetBytes(fileBegin);
                stream.Write(bytes, 0, bytes.Length);
                ////传文件数据
                stream.Write(item.Value, 0, item.Value.Length);
                //传换行数据
                byte[] LFBytes = Encoding.UTF8.GetBytes("\r\n");
                stream.Write(LFBytes, 0, LFBytes.Length);
            }
            //传参数数据
            StringBuilder sb_params = new StringBuilder();
            foreach (string key in paras.Keys)
            {
                sb_params.Append("--" + boundary + "\r\n");
                sb_params.Append("Content-Disposition: form-data; name=\"" + key + "\"\r\n\r\n");
                sb_params.Append(paras[key] + "\r\n");
            }
            byte[] paramsBytes = Encoding.UTF8.GetBytes(sb_params.ToString());
            stream.Write(paramsBytes, 0, paramsBytes.Length);
            //结束标记
            byte[] byte1 = Encoding.UTF8.GetBytes("--" + boundary + "--");//文件结束标志prefix很重要
            stream.Write(byte1, 0, byte1.Length);
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        return reader.ReadToEndAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

}
