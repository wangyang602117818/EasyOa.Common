using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyOa.Common
{
    /// <summary>
    /// 进制转换类
    /// </summary>
    public static class NumberExtentions
    {
        /// <summary>
        /// 十进制转二进制
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToBinary(this Int32 value)
        {
            return Convert.ToString(value, 2);
        }
        /// <summary>
        /// 十进制转十六进制
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToHex(this Int32 value)
        {
            return Convert.ToString(value, 16);
        }
        /// <summary>
        /// 二进制转十进制
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int32 BinaryToInt(this string value)
        {
            return Convert.ToInt32(value, 2);
        }
        /// <summary>
        /// 十六进制转十进制
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int32 HexToInt(this string value)
        {
            return Convert.ToInt32(value, 16);
        }
        /// <summary>
        /// 十进制数字转base64
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToBase64(this Int32 value)
        {
            byte[] bits = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(bits);
            int ofs = 0;
            while (bits[ofs] == 0 && ofs < bits.Length) ofs++;
            return Convert.ToBase64String(bits, ofs, bits.Length - ofs);
        }
        /// <summary>
        /// base64转int，前提是该base64本身就是int类型转过来的
        /// </summary>
        /// <param name="base64Value"></param>
        /// <returns></returns>
        public static Int32 Base64ToInt(this string base64Value)
        {
            byte[] intBytes = new byte[4];
            byte[] bits = Convert.FromBase64String(base64Value);
            bits.CopyTo(intBytes, intBytes.Length - bits.Length);
            if (BitConverter.IsLittleEndian) Array.Reverse(intBytes);
            return BitConverter.ToInt32(intBytes, 0);
        }
        /// <summary>
        /// 数组转int，
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Int32 BytesToInt(this Byte[] bytes)
        {
            byte[] intBytes = new byte[4];
            bytes.CopyTo(intBytes, intBytes.Length - bytes.Length);
            if (BitConverter.IsLittleEndian) Array.Reverse(intBytes);
            return BitConverter.ToInt32(intBytes, 0);
        }
    }
}
