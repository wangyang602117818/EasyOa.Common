using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyOa.Common
{
    public static class AppConfig
    {
        /// <summary>
        /// 网站跟目录  AppDomain.CurrentDomain.SetupInformation.ApplicationBase
        /// </summary>
        public static string BasePath = AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// 获取当前系统时间戳
        /// </summary>
        public static uint CurrentTimeStamp
        {
            get { return (uint)((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000); }
        }
        /// <summary>
        /// 获取配置文件节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfig(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

    }
}
