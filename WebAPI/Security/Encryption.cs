using System.Security.Cryptography;
using System.Text;
using System;
namespace WebAPI.Security
{
    public class Encryption
    {
        public string RSAEncrypt(string DataToEncrypt, string RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;

                var data = GetBytes(DataToEncrypt);

                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.FromXmlString(RSAKeyInfo);

                    encryptedData = RSA.Encrypt(data, DoOAEPPadding);
                }
                return Convert.ToBase64String(encryptedData);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        
        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
