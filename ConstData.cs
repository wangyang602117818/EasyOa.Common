using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyOa.Common
{
    public static class ConstData
    {
        public static string UploadFilePath = AppConfig.BasePath + "WebResource/uploadfile/";
        static ConstData()
        {
            if (!Directory.Exists(UploadFilePath)) Directory.CreateDirectory(UploadFilePath);
        }
    }
}
