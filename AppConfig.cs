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
        public static string basePath = AppDomain.CurrentDomain.BaseDirectory;
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
