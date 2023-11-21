using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numeric;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AplicationDsppk
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private double getEffRate(double CP_TENOR, double CP_LIMIT, double CP_INTEREST_FLAT, string CP_ANGSURAN_TYPE)
        {
            double fv = 0, pmt = 0;
            PaymentDue typ;
            double nper = 0.0;
            nper = CP_TENOR;

            double pv = 0.0;
            pv = CP_LIMIT;

            double interest = 0.0;
            interest = CP_INTEREST_FLAT;

            string tipe = CP_ANGSURAN_TYPE;

            double bungapinjaman = pv * interest / 100 * nper / 12;
            double totalpinjaman = pv + bungapinjaman;
            double CP_ANGSURAN = 0.0;
            pmt = totalpinjaman / nper;
            CP_ANGSURAN = pmt;
            pv = pv * -1;

            if (tipe == "1")
                typ = PaymentDue.BeginningOfPeriod;
            else typ = PaymentDue.EndOfPeriod;
            double efektif = Financial.Rate(nper, pmt, pv, fv, typ) * 100 * 12;
            double CP_INTEREST = 0.0;
            CP_INTEREST = efektif;

            return CP_INTEREST;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                double CP_TENOR = 0.0;
                if (txtCP_TENOR.Text.ToString() != null && txtCP_TENOR.Text.ToString() != "")
                    CP_TENOR = double.Parse(txtCP_TENOR.Text.ToString());

                double CP_LIMIT = 0.0;
                if (txtCP_LIMIT.Text.ToString() != null && txtCP_LIMIT.Text.ToString() != "")
                    CP_LIMIT = double.Parse(txtCP_LIMIT.Text.ToString());

                double CP_INTEREST_FLAT = 0.0;
                if (txtCP_INTEREST_FLAT.Text.ToString() != null && txtCP_INTEREST_FLAT.ToString() != "")
                    CP_INTEREST_FLAT = double.Parse(txtCP_INTEREST_FLAT.Text.ToString());

                string CP_ANGSURAN_TYPE = "";
                if (txtCP_ANGSURAN_TYPE.Text.ToString() != null && txtCP_ANGSURAN_TYPE.Text.ToString() != "")
                    CP_ANGSURAN_TYPE = txtCP_ANGSURAN_TYPE.Text.ToString();

                txtCP_INTEREST.Text = "" + getEffRate(CP_TENOR, CP_LIMIT, CP_INTEREST_FLAT, CP_ANGSURAN_TYPE);
            }
            catch
            {
                txtCP_INTEREST.Text = "ERROR";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string AWAL = "";
                AWAL = txtAwal.Text.ToString();
                txtEncript.Text = encriptSTR(AWAL);
            }
            catch
            {
                txtCP_INTEREST.Text = "ERROR";
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string AWAL = "";
                AWAL = txtAwal.Text.ToString();
                txtDecript.Text = decryptSTR(AWAL);
            }
            catch
            {
                txtCP_INTEREST.Text = "ERROR";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string AWAL = "";
                AWAL = txtAwal.Text.ToString();
                txtEncript.Text = encriptStr2(AWAL);
            }
            catch
            {
                txtCP_INTEREST.Text = "ERROR";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                string AWAL = "";
                AWAL = txtAwal.Text.ToString();
                txtDecript.Text = decryptStr2(AWAL);
            }
            catch
            {
                txtCP_INTEREST.Text = "ERROR";
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string AWAL = "";
            AWAL = txtAwal.Text.ToString();
            try
            {
                txtEncript.Text = EncodePassword(AWAL, "tWWpbhHJ3/wkPXH5E6kyrA==");
            }
            catch
            {
                txtCP_INTEREST.Text = "ERROR";
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                string AWAL = "";
                AWAL = txtAwal.Text.ToString();
                txtEncript.Text = DecodePassword(AWAL, "tWWpbhHJ3/wkPXH5E6kyrA==");
            }
            catch
            {
                txtCP_INTEREST.Text = "ERROR";
            }
        }


        private string EncodePassword(string pass, string salt)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(pass);
            byte[] array = Convert.FromBase64String(salt);
            byte[] array2 = null;
            HashAlgorithm hashAlgorithm = HashAlgorithm.Create("SHA1");

            byte[] array5 = new byte[array.Length + bytes.Length];
            Buffer.BlockCopy(array, 0, array5, 0, array.Length);
            Buffer.BlockCopy(bytes, 0, array5, array.Length, bytes.Length);
            array2 = hashAlgorithm.ComputeHash(array5);

            return Convert.ToBase64String(array2);
        }

        private string DecodePassword(string password, string salt)
        {
            byte[] bytes = Convert.FromBase64String(password);
            byte[] array4 = Convert.FromBase64String(salt);
            HashAlgorithm hashAlgorithm = HashAlgorithm.Create("SHA1");
            byte[] array5 = new byte[bytes.Length - array4.Length];
            Buffer.BlockCopy(bytes, 0, array5, 0, array5.Length);
            array5 = hashAlgorithm.ComputeHash(bytes);
            return Convert.ToBase64String(array5);
        }
        TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider();

        public string DecryptData(string encryptedtext, string salt)
        {

            // Convert the encrypted text string to a byte array.
            byte[] encryptedBytes = Convert.FromBase64String(encryptedtext);
            byte[] array4 = Convert.FromBase64String(salt);

            // Create the stream.
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            // Create the decoder to write to the stream.
            CryptoStream decStream = new CryptoStream(ms, tripleDes.CreateDecryptor(), CryptoStreamMode.Write);

            // Use the crypto stream to write the byte array to the stream.
            decStream.Write(encryptedBytes, 0, encryptedBytes.Length);
            decStream.FlushFinalBlock();

            // Convert the plaintext stream to a string.
            return System.Text.Encoding.Unicode.GetString(ms.ToArray());
        }

        /* public byte[] EncryptOrDecryptData(bool encrypt, byte[] buffer, bool useLegacyMode)
         {
             return MachineKeySection.EncryptOrDecryptData(encrypt, buffer, null, 0, buffer.Length, false, useLegacyMode, IVType.None, false);
         }*/


        public static string decryptStr2(string encryptedConnStr)
        {
            string decpwd = "";

            int a = 2;
            if (encryptedConnStr.Length <= a)
            {
                a = 1;
            }

            for (int i = a; i < encryptedConnStr.Length; i++)
            {
                char chr = (char)(encryptedConnStr[i] - 2);
                decpwd += new string(chr, 1);
            }

            return decpwd;
        }

        public static string encriptStr2(string decryptConnStr)
        {
            string connStr, decpwd = "";
            for (int i = 0; i < decryptConnStr.Length; i++)
            {
                char chr = (char)(decryptConnStr[i] + 2);

                decpwd += new string(chr, 1);
            }

            if (decpwd.Length > 0)
            {
                int a = 1;
                if (decpwd.Length > a)
                {
                    a = 2;
                }

                decpwd = decpwd.Substring(0, a) + decpwd;

                connStr = decryptConnStr.Replace(decryptConnStr, decpwd);
            }
            else
            {
                connStr = decryptConnStr;
            }

            return connStr;
        }

        public static string decryptConnStr(string encryptedConnStr)
        {
            string connStr, encpwd, decpwd = "";
            int pos1, pos2;

            int a = 2;
            if (encryptedConnStr.Length <= a)
            {
                a = 1;
            }

            for (int i = a; i < encryptedConnStr.Length; i++)
            {
                char chr = (char)(encryptedConnStr[i] - 2);
                decpwd += new string(chr, 1);
            }
            connStr = encryptedConnStr.Replace(encryptedConnStr, decpwd);
            return connStr;
        }

        public static string encriptSTR(string decryptConnStr)
        {
            string connStr, decpwd = "";
            for (int i = 0; i < decryptConnStr.Length; i++)
            {
                char chr = (char)(decryptConnStr[i] + 4);
                decpwd += new string(chr, 1);
            }

            if (decpwd.Length > 0)
            {
                int a = 1;
                if (decpwd.Length > a)
                {
                    a = 2;
                }
                decpwd = decpwd.Substring(0, a) + decpwd;

                connStr = decryptConnStr.Replace(decryptConnStr, decpwd);
            }
            else
            {
                connStr = decryptConnStr;
            }

            return connStr;
        }

        public static string decryptSTR(string encryptedConnStr)
        {
            string connStr, decpwd = "";

            int a = 2;
            if (encryptedConnStr.Length <= a)
            {
                a = 1;
            }

            for (int i = a; i < encryptedConnStr.Length; i++)
            {
                char chr = (char)(encryptedConnStr[i] - 4);
                decpwd += new string(chr, 1);
            }
            connStr = encryptedConnStr.Replace(encryptedConnStr, decpwd);
            return connStr;
        }

        public static string keyStr = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
        public static string Encrypt(string PlainText)
        {
            RijndaelManaged aes = new RijndaelManaged();
            aes.BlockSize = 128;
            aes.KeySize = 256;

            // It is equal in java 
            /// Cipher _Cipher = Cipher.getInstance("AES/CBC/PKCS5PADDING");    
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            byte[] keyArr = Convert.FromBase64String(keyStr);
            byte[] KeyArrBytes32Value = new byte[32];
            Array.Copy(keyArr, KeyArrBytes32Value, 32);

            // Initialization vector.   
            // It could be any value or generated using a random number generator.
            byte[] ivArr = { 1, 2, 3, 4, 5, 6, 6, 5, 4, 3, 2, 1, 7, 7, 7, 7 };
            byte[] IVBytes16Value = new byte[16];
            Array.Copy(ivArr, IVBytes16Value, 16);

            aes.Key = KeyArrBytes32Value;
            aes.IV = IVBytes16Value;

            ICryptoTransform encrypto = aes.CreateEncryptor();

            byte[] plainTextByte = ASCIIEncoding.UTF8.GetBytes(PlainText);
            byte[] CipherText = encrypto.TransformFinalBlock(plainTextByte, 0, plainTextByte.Length);
            return Convert.ToBase64String(CipherText);

        }

    }
}
