using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace mBillsTest
{
    class APICalls
    {
        AuthHeaderGenerator authGen;
        HttpClient httpClient;

        public APICalls(string apiKey, string secretKey) {
            authGen = new AuthHeaderGenerator(apiKey, secretKey);
            httpClient = new HttpClient();
        }

        public void setAuthenticationHeader(string url) {
            httpClient.DefaultRequestHeaders.Authorization = authGen.getAuthenticationHeaderValue(url);
        }

        public async void testConnection() {

            string url = "http://private-anon-68d8958991-mbillsquickpaymentapi.apiary-mock.com/API/v1/system/test";
            setAuthenticationHeader(url);

            string response = await httpClient.GetStringAsync(url);
            Console.WriteLine(response);
        }
    }
}
