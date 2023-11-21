using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Numeric;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using AplicationDsppk.Model;
using FileHelpers;

namespace AplicationDsppk
{
    public partial class SendMail : Form
    {
        public SendMail()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            ModelReqData request = new ModelReqData();

            request.REFNO = TXT_5.Text;
            request.MSISDN = TXT_3.Text;
            if (TXT_4.Text.Length > 0 & TXT_4.Text.Length > 450)
            {
                request.MESSAGE = TXT_4.Text.ToString().Substring(450);
            }
            else
            {
                request.MESSAGE = "";
            }
            
            request.MASKING = TXT_6.Text;
            request.USERID = TXT_1.Text;
            request.PASSWORD = TXT_2.Text;

            try
            {
                var response = SendSMS(request, TXT_7.Text);
            }
            catch(Exception ex) {
                MessageBox.Show("Error loh " + ex.Message);
            }
        }

        private ModelResData SendSMS(ModelReqData data, string URL)
        {
            ModelResData resp = new ModelResData();

            try
            {
                var str = new JavaScriptSerializer().Serialize(data);

                TXT_OUTPUT.Text = str;
                var respnd = PostHttp<JsData>(str, URL, 12000);
                
                resp.STATUS = respnd.data[0];
                resp.TRXID = respnd.data[1];

                TXT_OUTPUT.Text = respnd.ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
            return resp;
        }

        static T PostHttp<T>(string requestData, string uri, int timeOut)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslPolicyErrors) => true;

                byte[] data = System.Text.Encoding.ASCII.GetBytes(requestData);

                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                    delegate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                            System.Security.Cryptography.X509Certificates.X509Chain chain,
                                            System.Net.Security.SslPolicyErrors sslPolicyErrors)
                    {
                        return true; // **** Always accept
                    };

                int timeout = timeOut;
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = WebRequestMethods.Http.Post;
                request.ContentType = "application/json";
                request.KeepAlive = false;
                request.Timeout = timeout;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string result = reader.ReadToEnd();

                        var ser = new ServiceStack.Text.JsonSerializer<T>();
                        T resp = ser.DeserializeFromString(result);

                        return resp;
                    }
                }
            }
            catch
            {
                return default(T);
            }
        }
    }

    [FixedLengthRecord(FileHelpers.FixedMode.AllowMoreChars)]
    public class ModelResData
    {
        [FieldOrder(1)]
        [FieldFixedLength(18)]
        public string TRXID;

        [FieldOrder(2)]
        [FieldFixedLength(3)]
        public string STATUS;

    }

    [FixedLengthRecord(FileHelpers.FixedMode.AllowMoreChars)]
    public class ModelReqData
    {
        [FieldOrder(1)]
        [FieldFixedLength(30)]
        public string USERID;

        [FieldOrder(2)]
        [FieldFixedLength(30)]
        public string PASSWORD;

        [FieldOrder(3)]
        [FieldFixedLength(50)]
        public string MSISDN;

        [FieldOrder(4)]
        [FieldFixedLength(450)]
        public string MESSAGE;

        [FieldOrder(5)]
        [FieldFixedLength(18)]
        public string REFNO;

        [FieldOrder(6)]
        [FieldFixedLength(30)]
        public string MASKING;
    }
}
