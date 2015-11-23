using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyOa.Common
{
    public static class FileHelper
    {
        /// <summary>
        /// 读取文本文件，返回字典,其中文本中的第一个字符串作为key，后面的作为value
        /// </summary>
        /// <param name="fullpath">文件绝对路径</param>
        /// <param name="sign">文本分隔符</param>
        /// <returns></returns>
        public static Dictionary<string, string[]> ReadFileSplit(string fullpath, string sign = " ")
        {
            if (!File.Exists(fullpath)) return null;
            string[] lines = File.ReadAllLines(fullpath);
            Dictionary<string, string[]> dict = new Dictionary<string, string[]>();
            foreach (string line in lines)
            {
                string[] sword = line.Split(new string[] { sign }, StringSplitOptions.RemoveEmptyEntries);
                string[] word = new string[sword.Length - 1];
                Array.Copy(sword, 1, word, 0, sword.Length - 1);
                dict.Add(sword[0], word);
            }
            return dict;
        }
        /// <summary>
        /// 把字符串写入文件，文件不存在则创建，不会覆盖源文件内容
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="filename"></param>
        /// <param name="msg"></param>
        public static void WriteFile(string fullpath, string filename, string msg)
        {
            WriteFile(fullpath, filename, msg, true);
        }
        /// <summary>
        /// 吧字符串写入文件，并覆盖源文件内容
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="filename"></param>
        /// <param name="msg"></param>
        /// <param name="append">true:追加，false:覆盖</param>
        public static void WriteFile(string fullpath, string filename, string msg, bool append)
        {
            if (!Directory.Exists(fullpath)) Directory.CreateDirectory(fullpath);
            string fileName = Path.Combine(fullpath, filename);
            using (StreamWriter sw = new StreamWriter(fileName, append))
            {
                sw.WriteLine(msg);
            }
        }
        /// <summary>
        /// 在项目根目录创建文件夹写日志,文件夹为当前日期
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="msg"></param>
        public static void WriteFile(string filename, string msg)
        {
            string fullpath = AppConfig.BasePath + DateTime.Now.ToString("yyyyMMdd");
            WriteFile(fullpath, filename, msg);
        }
        /// <summary>
        /// 获取文件的真实类型，jpg/gif/png/bmp
        /// </summary>
        /// <param name="fileData"></param>
        /// <returns></returns>
        public static string GetFileSuffix(byte[] fileData)
        {
            if (fileData == null || fileData.Length < 10) return null;
            if (fileData[0] == 'G' && fileData[1] == 'I' && fileData[2] == 'F') return "GIF";
            if (fileData[1] == 'P' && fileData[2] == 'N' && fileData[3] == 'G') return "PNG";
            if (fileData[6] == 'J' && fileData[7] == 'F' && fileData[8] == 'I' && fileData[9] == 'F') return "JPG";
            if (fileData[0] == 'B' && fileData[1] == 'M') return "BMP";
            return null;
        }
    }
}
