using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ShippingSyncServer
{
    public static class CookieUtility
    {
        //默认密钥向量   
        private static byte[] _key1 =
            {
                0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90,
                0xAB, 0xCD, 0xEF
            };

        //默认密钥
        private const string StrKey = "o{Z_Dj(|qn4`+B(,%_>fOvA@a4rb1u}x";

        //盐值
        private const string SaltValue = "7446768A-64D4-4AFC-92C2-C4551F8BBD50";

        /// <summary>  
        /// AES加密算法  
        /// </summary>  
        /// <param name="plainText">明文字符串</param>  
        /// <returns>返回加密后的密文字节数组</returns>  
        public static string AESEncrypt(string plainText)
        {
            //分组加密算法  
            SymmetricAlgorithm des = Aes.Create();
            byte[] inputByteArray = Encoding.ASCII.GetBytes(plainText); //得到需要加密的字节数组      
            //设置密钥及密钥向量  
            des.Key = Encoding.UTF8.GetBytes(StrKey);
            des.IV = _key1;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            byte[] cipherBytes = ms.ToArray(); //得到加密后的字节数组  
            cs.Close();
            ms.Close();
            StringBuilder stringBuilder = new StringBuilder(cipherBytes.Length);

            foreach (var b in cipherBytes)
            {
                stringBuilder.Append(b + "-");
            }
            return stringBuilder.ToString().TrimEnd('-');
        }

        /// <summary>  
        /// AES解密  
        /// </summary>  
        /// <param name="cipherText">密文字节数组</param>  
        /// <returns>返回解密后的字符串</returns>  
        public static string AESDecrypt(string strCipherText)
        {
            try
            {
                string[] arrryCipherText = strCipherText.Split('-');
                List<byte> cipherText = new List<byte>();

                for (var i = 0; i < arrryCipherText.Length; i++)
                {
                    cipherText.Add(Convert.ToByte(arrryCipherText[i]));
                }

                SymmetricAlgorithm des = Rijndael.Create();
                des.Key = Encoding.UTF8.GetBytes(StrKey);
                des.IV = _key1;
                byte[] decryptBytes = new byte[cipherText.Count];
                MemoryStream ms = new MemoryStream(cipherText.ToArray());
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read);
                cs.Read(decryptBytes, 0, decryptBytes.Length);
                cs.Close();
                ms.Close();
                return Encoding.UTF8.GetString(decryptBytes).Replace("\0", "");

            }
            catch
            {
                return Guid.NewGuid().ToString();
            }
        }

        /// <summary>
        /// 获取令牌
        /// </summary>
        /// <returns></returns>
        public static string GetToken(string ID)
        {
            ID = ID + SaltValue;
            return Hash(ID);
        }

        #region private method Hash
        private static string Hash(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            var salt = Md5Hash(data) + Sha1Hash(data);

            return Sha1Hash(data + salt);
        }

        private static string Md5Hash(string password)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(password);

            var sha = MD5.Create();

            var hashedBuffer = sha.ComputeHash(buffer);

            StringBuilder stringBuilder = new StringBuilder(hashedBuffer.Length);

            foreach (var b in hashedBuffer)
            {
                stringBuilder.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            }

            var result = stringBuilder.ToString();

            return result;
        }

        private static string Sha1Hash(string password)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(password);

            var sha = SHA1.Create();

            var hashedBuffer = sha.ComputeHash(buffer);

            StringBuilder stringBuilder = new StringBuilder(hashedBuffer.Length);

            foreach (var b in hashedBuffer)
            {
                stringBuilder.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            }

            var result = stringBuilder.ToString();

            return result;
        }

        #endregion
    }
}
