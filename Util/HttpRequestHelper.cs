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
        /// 发送post请求,或put
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
        /// get请求,或delete
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
        /// <param name="fullFilePath">本文件路径</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public static string PostAttachLocalFile(string url, string fullFilePath, string paras)
        {
            if (!File.Exists(fullFilePath)) return null;
            byte[] fileBytes = File.ReadAllBytes(fullFilePath);
            return PostFile(url, fileBytes, Path.GetFileName(fullFilePath), paras);
        }
        /// <summary>
        /// http协议上传网络文件
        /// </summary>
        /// <param name="url">要上传文件的url地址</param>
        /// <param name="fileUrl">文件在网络的位置</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public static string PostAttachWebFile(string url, string fileUrl, string paras)
        {
            byte[] fileBytes;
            string fileName;
            try
            {
                fileBytes = new WebClient().DownloadData(fileUrl);
                fileName = Path.GetFileName(fileUrl);
            }
            catch (WebException ex)
            {
                LogHelper.WriteException(ex);
                return null;
            }
            if (fileBytes.Length > 0 && !string.IsNullOrEmpty(fileName))
            {
                return PostFile(url, fileBytes, fileName, paras);
            }
            return null;
        }
        private static string PostFile(string url, byte[] fileBytes, string fileName, string paras)
        {
            string boundary = "----Ij5ei4ae0ei4cH2ae0Ef1ei4Ij5gL6";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "post";
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            Stream stream = request.GetRequestStream();  //请求流
            //文件开始标记
            string fileBegin = "--" + boundary + "\r\nContent-Disposition: form-data;name=\"file\";filename=\"" + fileName + "\"\r\nContent-Type: application/octet-stream; charset=utf-8\r\n\r\n";
            byte[] bytes = Encoding.UTF8.GetBytes(fileBegin);
            stream.Write(bytes, 0, bytes.Length);
            ////传文件数据
            stream.Write(fileBytes, 0, fileBytes.Length);
            //传换行数据
            byte[] LFBytes = Encoding.UTF8.GetBytes("\r\n");
            stream.Write(LFBytes, 0, LFBytes.Length);
            //传参数数据
            NameValueCollection nameValueCollection = ParseRequestParams.ParseToNameValueCollection(paras);
            StringBuilder sb_params = new StringBuilder();
            foreach (string key in nameValueCollection.Keys)
            {
                sb_params.Append("--" + boundary + "\r\n");
                sb_params.Append("Content-Disposition: form-data; name=\"" + key + "\"\r\n\r\n");
                sb_params.Append(nameValueCollection[key] + "\r\n");
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
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteException(ex);
                return null;
            }
        }
    }

}
