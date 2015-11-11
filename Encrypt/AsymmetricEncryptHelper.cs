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
        public static string RSAEncode(string sourceString, string pubKey)
        {
            byte[] sourceBytes = Encoding.UTF8.GetBytes(sourceString);
            RSAParameters rsaParameters = ConvertFromPemPublicKey(pubKey);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(rsaParameters);
            byte[] result = rsa.Encrypt(sourceBytes, false);
            return Convert.ToBase64String(result);
        }
        public static string RSADecode(string secretString, string priKey)
        {
            byte[] dataInput = Convert.FromBase64String(secretString);  //待解密的字符串
            RSAParameters rsaParameters = ConvertFromPemPrivateKey(priKey);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(rsaParameters);
            byte[] result = rsa.Decrypt(dataInput, false);
            return Encoding.UTF8.GetString(result);
        }
        /// <summary>          
        /// 将pem格式公钥转换为RSAParameters         
        /// </summary> <param name="pemFileConent">pem公钥内容</param>         
        /// <returns>转换得到的RSAParamenters</returns>          
        public static RSAParameters ConvertFromPemPublicKey(string pemPubKey)
        {
            byte[] keyData = Convert.FromBase64String(pemPubKey);
            if (keyData.Length < 162) { throw new ArgumentException("pem file content is incorrect."); }
            byte[] pemModulus = new byte[128];
            byte[] pemPublicExponent = new byte[3];
            Array.Copy(keyData, 29, pemModulus, 0, 128);
            Array.Copy(keyData, 159, pemPublicExponent, 0, 3);
            RSAParameters para = new RSAParameters();
            para.Modulus = pemModulus; 
            para.Exponent = pemPublicExponent;
            return para;
        }
        /// <summary>          
        /// 将pem格式私钥转换为RSAParameters        
        /// </summary>          
        /// <param name="pemFileConent">pem私钥内容</param>        
        /// <returns>转换得到的RSAParamenters</returns>         
        public static RSAParameters ConvertFromPemPrivateKey(string pemPriKey)
        {
            byte[] keyData = Convert.FromBase64String(pemPriKey);
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;
            MemoryStream mem = new MemoryStream(keyData);
            BinaryReader binr = new BinaryReader(mem);  //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            twobytes = binr.ReadUInt16();
            if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                binr.ReadByte();    //advance 1 byte
            else if (twobytes == 0x8230)
                binr.ReadInt16();    //advance 2 bytes
            else
                throw new Exception("异常");

            twobytes = binr.ReadUInt16();
            if (twobytes != 0x0102) //version number
                throw new Exception("异常");
            bt = binr.ReadByte();
            if (bt != 0x00)
                throw new Exception("异常");


            //------ all private key components are Integer sequences ----
            elems = GetIntegerSize(binr);
            MODULUS = binr.ReadBytes(elems);

            elems = GetIntegerSize(binr);
            E = binr.ReadBytes(elems);

            elems = GetIntegerSize(binr);
            D = binr.ReadBytes(elems);

            elems = GetIntegerSize(binr);
            P = binr.ReadBytes(elems);

            elems = GetIntegerSize(binr);
            Q = binr.ReadBytes(elems);

            elems = GetIntegerSize(binr);
            DP = binr.ReadBytes(elems);

            elems = GetIntegerSize(binr);
            DQ = binr.ReadBytes(elems);

            elems = GetIntegerSize(binr);
            IQ = binr.ReadBytes(elems);
            RSAParameters RSAparams = new RSAParameters();
            RSAparams.Modulus = MODULUS;
            RSAparams.Exponent = E;
            RSAparams.D = D;
            RSAparams.P = P;
            RSAparams.Q = Q;
            RSAparams.DP = DP;
            RSAparams.DQ = DQ;
            RSAparams.InverseQ = IQ;
            return RSAparams;
        }
        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)		//expect integer
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();	// data size in next byte
            else
                if (bt == 0x82)
                {
                    highbyte = binr.ReadByte();	// data size in next 2 bytes
                    lowbyte = binr.ReadByte();
                    byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                    count = BitConverter.ToInt32(modint, 0);
                }
                else
                {
                    count = bt;		// we already have the data size
                }
            while (binr.ReadByte() == 0x00)
            {	//remove high order zeros in data
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);		//last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }
    }
}
