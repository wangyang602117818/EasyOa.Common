using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EasyOa.Common
{
    /// <summary>
    /// 非对称加密算法
    /// </summary>
    public static class AsymmetricEncryptHelper
    {
        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="sourceString">需要加密的字符串</param>
        /// <param name="key">base64形式的key</param>
        /// <returns></returns>
        public static string RSAEncode(string sourceString, string key)
        {
            byte[] sourceBytes = Encoding.UTF8.GetBytes(sourceString);
            byte[] keyBytes = Convert.FromBase64String(key);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportCspBlob(keyBytes);
            byte[] result = rsa.Encrypt(sourceBytes, false);
            return Convert.ToBase64String(result);
        }
        public static string RSADecode(string secretString, string key)
        {
            byte[] dataInput = Convert.FromBase64String(secretString);  //待解密的字符串
            byte[] keyBytes = Convert.FromBase64String(key);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportCspBlob(keyBytes);
            byte[] result = rsa.Decrypt(dataInput, false);
            return Encoding.UTF8.GetString(result);
        }
    }
}
