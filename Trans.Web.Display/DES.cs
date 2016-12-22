using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Trans.Web.Display
{
    public class DES
    {
        private static string CookieDESKEY = ConfigurationManager.AppSettings["COOKIE_DESKEY"] ?? "TRANSKEY";
        private static string CookieDESIV= ConfigurationManager.AppSettings["COOKIE_DESIV"] ?? "TRANS_IV";
        public static string Encrypt(string SrcStr)
        {
            try
            {
                byte[] btKey = Encoding.UTF8.GetBytes(CookieDESKEY);
                byte[] btIV = Encoding.UTF8.GetBytes(CookieDESIV);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] inData = Encoding.UTF8.GetBytes(SrcStr);
                    try
                    {
                        using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write))
                        {
                            cs.Write(inData, 0, inData.Length);
                            cs.FlushFinalBlock();
                        }
                        return Convert.ToBase64String(ms.ToArray());
                    }
                    catch
                    {
                        return SrcStr;
                    }
                }
            }
            catch { }

            return "";
        }
        public static string Decrypt(string SrcStr)
        {
            byte[] btKey = Encoding.UTF8.GetBytes(CookieDESKEY);
            byte[] btIV = Encoding.UTF8.GetBytes(CookieDESIV);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            using (MemoryStream ms = new MemoryStream())
            {
                SrcStr = SrcStr.Replace(" ", "+");
                
                byte[] inData = Convert.FromBase64String(SrcStr);
                try
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write))
                    {
                        cs.Write(inData, 0, inData.Length);
                        cs.FlushFinalBlock();
                    }
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
                catch
                {
                    return SrcStr;
                }
            }
        }
    }
}