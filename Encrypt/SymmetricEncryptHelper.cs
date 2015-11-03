using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EasyOa.Common
{
    /// <summary>
    /// 对称加密算法
    /// </summary>
    public static class SymmetricEncryptHelper
    {
        //初始化向量
        private static byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        /// <summary>
        /// Aes加密算法
        /// </summary>
        /// <param name="sourceString"></param>
        /// <param name="key">长度：16/24/32</param>
        /// <returns></returns>
        public static string AesEncode(string sourceString, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] valueBytes = Encoding.UTF8.GetBytes(sourceString);
            using (Rijndael rijndael = Rijndael.Create())
            {
                rijndael.Mode = CipherMode.ECB;
                using (ICryptoTransform transform = rijndael.CreateEncryptor(keyBytes, IV))
                {
                    MemoryStream memoryStream = new MemoryStream();
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(valueBytes, 0, valueBytes.Length); //往memoryStream中写入流
                        cryptoStream.FlushFinalBlock();
                        return Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }
        }
        /// <summary>
        /// Aes解密算法
        /// </summary>
        /// <param name="secretString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AesDecode(string secretString, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Convert.FromBase64String(secretString);
            using (Rijndael rijndael = Rijndael.Create())
            {
                rijndael.Mode = CipherMode.ECB;
                using (ICryptoTransform transform = rijndael.CreateDecryptor(keyBytes, IV))  //创建一个解密器
                {
                    MemoryStream memoryStream = new MemoryStream();
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(inputBytes, 0, inputBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        return Encoding.UTF8.GetString(memoryStream.ToArray());
                    }
                }
            }
        }
        /// <summary>
        /// DES加密，8位字符串秘钥（64位，其中第8、16、24、32、40、48、56、64位是校验位）,校验位不参与密码计算
        /// 所以可能会出现，不同的key加密出来的字符串是一样的，不同的key可以解密相同的字符串
        /// </summary>
        /// <param name="sourceString"></param>
        /// <param name="key">字符串长度为8的key</param>
        /// <returns>返回base64编码的密文</returns>
        public static string DESEncode(string sourceString, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] valueBytes = Encoding.UTF8.GetBytes(sourceString);
            using (DES des = DES.Create())
            {
                des.Mode = CipherMode.ECB;
                using (ICryptoTransform transform = des.CreateEncryptor(keyBytes, IV))  //创建一个加密器
                {
                    MemoryStream memoryStream = new MemoryStream();
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(valueBytes, 0, valueBytes.Length); //往memoryStream中写入流
                        cryptoStream.FlushFinalBlock();
                        return Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }
        }
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="secretString">base64加密字符串</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string DESDecode(string secretString, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Convert.FromBase64String(secretString);
            using (DES des = DES.Create())
            {
                des.Mode = CipherMode.ECB;
                using (ICryptoTransform transform = des.CreateDecryptor(keyBytes, IV))  //创建一个解密器
                {
                    MemoryStream memoryStream = new MemoryStream();
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(inputBytes, 0, inputBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        return Encoding.UTF8.GetString(memoryStream.ToArray());
                    }
                }
            }
        }
        /// <summary>
        /// 3DES加密，为DES向AES过度的版本
        /// </summary>
        /// <param name="sourceString">要加密的字符串</param>
        /// <param name="key">字符串长度为24的key或16的key</param>
        /// <returns>返回base64编码的密文</returns>
        public static string TripleDESEncode(string sourceString, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] valueBytes = Encoding.UTF8.GetBytes(sourceString);
            using (TripleDES tripleDes = TripleDES.Create())
            {
                tripleDes.Mode = CipherMode.ECB; //设置后初始化向量失效
                using (ICryptoTransform transform = tripleDes.CreateEncryptor(keyBytes, IV))
                {
                    MemoryStream memoryStream = new MemoryStream();
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(valueBytes, 0, valueBytes.Length); //往memoryStream中写入流
                        cryptoStream.FlushFinalBlock();
                        return Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }
        }
        /// <summary>
        /// 3DES解密
        /// </summary>
        /// <param name="secretString">密文</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string TripleDESDecode(string secretString, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Convert.FromBase64String(secretString);
            using (TripleDES tripleDes = TripleDES.Create())
            {
                tripleDes.Mode = CipherMode.ECB;
                using (ICryptoTransform transform = tripleDes.CreateDecryptor(keyBytes, IV))  //创建一个解密器
                {
                    MemoryStream memoryStream = new MemoryStream();
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(inputBytes, 0, inputBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        return Encoding.UTF8.GetString(memoryStream.ToArray());
                    }
                }
            }
        }

    }
}
