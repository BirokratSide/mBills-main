using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Cache;
using System.IO;

namespace mBillsTest
{
    public class Program
    {
        // variables
        static string urlBase = "http://private-anon-68d8958991-mbillsquickpaymentapi.apiary-mock.com/";
        static string apiRoute = "API/v1/system/test";

        static string apiKey = "kurac";
        static string secretKey = "kurac";

        static APICalls api;

        // logic
        static void Main(string[] args)
        {

            api = new APICalls(apiKey, secretKey);
            api.testConnection();

            Console.ReadLine();
        }

        static void test() {
            Console.WriteLine(basicGetRequest(urlBase + apiRoute));
        }

        static void testwebhook() {
            
        }


        #region [auxiliary]
        static string basicGetRequest(string url) {

            string strData = string.Empty;
            byte[] arrData = new byte[0];

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:10.0.2) Gecko/20100101 Firefox/10.0.2";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "GET";
            using (HttpWebResponse objWA = (HttpWebResponse)request.GetResponse())
            {
                using (Stream srm = objWA.GetResponseStream())
                {
                    int intBytesRead = 0;
                    while (true)
                    {
                        byte[] arrBuff = new byte[1024];
                        intBytesRead = srm.Read(arrBuff, 0, 1024);
                        if (intBytesRead == 0) break;
                        int intNewSize = arrData.Length + intBytesRead;
                        byte[] arrTemp = new byte[intNewSize];
                        Array.Copy(arrData, 0, arrTemp, 0, arrData.Length);
                        Array.Copy(arrBuff, 0, arrTemp, arrData.Length, intBytesRead);
                        arrData = arrTemp;
                    }
                }
            }

            strData = Encoding.UTF8.GetString(arrData);
            return strData;
        }
        #endregion

    }
}
