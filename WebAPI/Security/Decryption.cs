using System.Security.Cryptography;

namespace WebAPI.Security
{
    public class Decryption
    {
        public string RSADecrypt(string DataToDecrypt, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;
                string privateKey = "<RSAKeyValue><Modulus>sU7SHN6d/A0cuBDYxnbJRm3cwwFcQotrwRqXBpaLLxE+/xZqoBP8buMAXUf3jJKDVmkiCFV4lA7OlvX2ENw0yubsCYwYJfce8/qbfijl7bir3Jhf669gGuFqB5awQU+hccciU12D+fJfkn0Uak/j2QLkhWQ8puTBe/IuOB3/y1k=</Modulus><Exponent>AQAB</Exponent><P>6lO0otRNFb+xtWH2vhSgauFBvmvrDIr1FNJLPd37twW40DLIyG1R9vYtZ+NtPF3Bq48hY1+JG+ozJyg/tdlPnw==</P><Q>wbUKpjJd34I5yt9D68beMS8uhGY0iNJb5FbMMLrA/jyHkkMnzH8jqzxgbVc2qGTwILHtGqLC3QUtTccdr9SiBw==</Q><DP>WQwwRASbup0bYkt1IezivVsWyc9nBmA0HJKB52PZSUcSYYQ7CveIQ2pv/N/PXjZe59w/muzuRL5ua/3+oBOSDw==</DP><DQ>TIXauSE4mG13qz9cM66SZFfBRqihpFOF3cS0UaPVThpXbF5/QHgRfToS1d8YRpnpiuD/TAB0fp/m78zzW1zizQ==</DQ><InverseQ>a7dXjvUBHtIg+XEWcw1F9S+Pvy/9depeHIL787JvpfquFIYvaSefbGxASa5RJQ4yC4O25qyU5R9sS4oAcfVR4Q==</InverseQ><D>TAEqpj9zX4FLZ0epdOGkg/FCKcFiiA/1v9AjHyrXPPTamURFrpsCoZHjLRlVb0e6zwbAFOx2hJkYS7Php/aNFhmNJt87FgyFcEXqfXZuxZRwt26xGuWnfl91vryg4DHATo8GKk4sTQaFVbKR8rEnhASy65vjNehSp3/3upA6XeU=</D></RSAKeyValue>";
                
                var dataToDecryptBytes = Convert.FromBase64String(DataToDecrypt);
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.FromXmlString(privateKey);

                    decryptedData = RSA.Decrypt(dataToDecryptBytes, DoOAEPPadding);
                }
                return GetString(decryptedData);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }
}
