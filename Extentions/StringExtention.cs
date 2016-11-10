using Microsoft.International.Converters.PinYinConverter;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyOa.Common
{
    public static class StringExtention
    {
        /// <summary>
        /// Md5计算
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToMD5(this string str)
        {
            return HashEncryptHelper.StringMd5(str);
        }
        /// <summary>
        /// 拼音转换
        /// </summary>
        /// <param name="str"></param>
        /// <param name="simple"></param>
        /// <returns></returns>
        public static string ToSpell(this string str, bool simple = false)
        {
            if (string.IsNullOrEmpty(str)) return "";
            string fullPath = AppConfig.BasePath + AppConfig.GetConfig("pinypath");
            Dictionary<string, string[]> dict = FileHelper.ReadFileSplit(fullPath, "|");
            if (dict != null && dict.Count > 0)
            {
                foreach (string key in dict.Keys)
                {
                    string[] value = dict[key];
                    if (str.Contains(key))
                    {
                        if (simple)
                        {
                            str = value.Length > 1
                                      ? str.Replace(key, value[1])
                                      : str.Replace(key, value[0].Substring(0, 1));
                        }
                        else
                        {
                            str = str.Replace(key, value[0]);
                        }
                    }
                }
            }
            List<string> res = new List<string>();
            foreach (char c in str)
            {
                if (ChineseChar.IsValidChar(c))
                {
                    string piny = new ChineseChar(c).Pinyins[0];
                    piny = piny.Substring(0, piny.Length - 1);
                    res.Add(piny.ToLower());
                }
                else
                {
                    res.Add(c.ToString());
                }
            }
            return simple ? string.Join("", res.Select(r => r.Substring(0, 1))) : string.Join("", res);
        }
        /// <summary>
        /// 字符串正则替换扩展
        /// </summary>
        /// <param name="str"></param>
        /// <param name="regexp"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string RegexReplace(this string str, string regexp, string replacement)
        {
            return Regex.Replace(str, regexp, replacement);
        }
        /// <summary>
        /// 将字符串中非ascii编码转unicode编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string NotAsciiToUnicode(this string str)
        {
            if (string.IsNullOrEmpty(str)) return "";
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((int)c > 127)
                {
                    sb.Append("\\u" + ((int)c).ToString("x"));
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 把字符串中unicode编码转成字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UnicodeToChar(this string str)
        {
            if (string.IsNullOrEmpty(str)) return "";
            return Regex.Unescape(str);  //方式一
            //return Regex.Replace(str, @"\\u(\w{4})", (match) => ((char)int.Parse(match.Groups[1].Value, NumberStyles.HexNumber)).ToString());   //方式二

        }
        /// <summary>
        /// 字符串转base64
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToBase64(this string str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(buffer);
        }
        /// <summary>
        /// 字符串转UTF8字节数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] StrToBuffer(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }
        /// <summary>
        /// base64字符串转UTF8字节数组
        /// </summary>
        /// <param name="base64Str"></param>
        /// <returns></returns>
        public static byte[] Base64StrToBuffer(this string base64Str)
        {
            return Convert.FromBase64String(base64Str);
        }
        /// <summary>
        /// 字符串空、null值判断
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
        /// <summary>
        /// 字符串转int
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt(this string str)
        {
            return Convert.ToInt32(str);
        }
        /// <summary>
        /// TimeStamp字符串转DateTime
        /// </summary>
        /// <param name="stringTimeStamp">unix时间戳</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string stringTimeStamp)
        {
            double timeStamp = Convert.ToDouble(stringTimeStamp);
            return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(timeStamp);
        }

        /// <summary>
        /// 取字符串,超过长度就取最大的值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SubStringNoException(this string str, int startIndex, int length)
        {
            if (length > str.Length - startIndex) length = str.Length - startIndex;
            return str.Substring(startIndex, length);
        }

        public static Stream ToStream(this string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            return new MemoryStream(bytes);
        }

        public static string ToStr(this Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

    }
}
