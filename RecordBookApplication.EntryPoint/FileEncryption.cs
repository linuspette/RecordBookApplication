using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace RecordBookApplication.EntryPoint
{
    public class FileEncryption
    {
        //  Call this function to remove the key from memory after use for security
        [DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
        public static extern bool ZeroMemory(IntPtr Destination, int length);

        private static string _password = "turbobananer";

        //Creates random salt
        public static byte[] GenereateRandomSalt()
        {
            byte[] data = new byte[32];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                for (int i = 0; i < 10; i++)
                {
                    rng.GetBytes(data);
                }
            }

                return data;
        }

        //User interaction
        public static void Encrypt(string inputFile, string password)
        {
            FileEncrypt(inputFile, password);
        }        
        public static void EncryptAllFiles(string[] databases)
        {
            for (int i = 0; i < databases.Length-1; i++)
            {
                FileEncrypt(databases[i], _password);
            }
        }        
        public static void DeCryptAllFiles(string[] databases)
        {
            for (int i = 0; i < databases.Length-1; i++)
            {
                FileDecrypt($"{databases[i]}.aes",databases[i], _password);
            }
        }
        public static void DeCrypt(string inputFile, string outputFile, string password)
        {
            FileDecrypt(inputFile, outputFile, password);
        }

        //Methods that handle cryptation
        private static void FileEncrypt(string inputFile, string password)
        {
            try
            {
                //Generates random salt
                byte[] salt = GenereateRandomSalt();

                //Creates output file name
                FileStream fsCrypt = new FileStream(inputFile + ".aes", FileMode.Create);

                //Convert password string to byte array
                byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);

                //Set Rijndael symmetric encryption
                RijndaelManaged AES = new RijndaelManaged();
                AES.KeySize = 256;
                AES.BlockSize = 128;
                AES.Padding = PaddingMode.PKCS7;

                //Repeatedly hashes user password along with the salt
                var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
                AES.Key = key.GetBytes(AES.KeySize / 8);
                AES.IV = key.GetBytes(AES.BlockSize / 8);

                AES.Mode = CipherMode.CFB;


                fsCrypt.Write(salt, 0, salt.Length);

                CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateEncryptor(), CryptoStreamMode.Write);
                FileStream fsIn = new FileStream(inputFile, FileMode.Open);

                byte[] buffer = new byte[1048576];
                int read;

                try
                {
                    while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        //Application.DoEvents();
                        cs.Write(buffer, 0, read);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    cs.Close();
                    fsIn.Close();
                    File.Delete(inputFile);
                    fsCrypt.Close();
                }

            }
            catch
            {
            }
        }
        private static void FileDecrypt(string inputFile, string outputFile, string password)
        {
            try
            {
                byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] salt = new byte[32];

                FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);
                fsCrypt.Read(salt, 0, salt.Length);

                RijndaelManaged AES = new RijndaelManaged();
                AES.KeySize = 256;
                AES.BlockSize = 128;
                var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
                AES.Key = key.GetBytes(AES.KeySize / 8);
                AES.IV = key.GetBytes(AES.BlockSize / 8);
                AES.Padding = PaddingMode.PKCS7;
                AES.Mode = CipherMode.CFB;

                CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateDecryptor(), CryptoStreamMode.Read);

                FileStream fsOut = new FileStream(outputFile, FileMode.Create);

                int read;
                byte[] buffer = new byte[1048576];

                try
                {
                    while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        fsOut.Write(buffer, 0, read);
                    }
                }
                catch (CryptographicException ex_CryptographicException)
                {
                    Console.WriteLine("CryptographicException error: " + ex_CryptographicException.Message);
                }
                try
                {
                    cs.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error by closing CryptoStream: " + ex.Message);
                }
                finally
                {
                    fsOut.Close();
                    fsCrypt.Close();
                    File.Delete(inputFile);
                }
            }
            catch
            {

            }

        }
    }
}
