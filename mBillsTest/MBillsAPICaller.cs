using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

using mBillsTest.structs;

namespace mBillsTest
{
    /*
    APICalls's public methods will call the mBills API. At instantiation, it also knows by itself how to set the authorization
    headers for the calls.
    */
    public class MBillsAPICaller
    {
        string apiRootPath = "";
        MBillsAuthHeaderGenerator authGen;
        HttpClient httpClient;


        public MBillsAPICaller(string apiRootPath, string apiKey, string secretKey) {
            authGen = new MBillsAuthHeaderGenerator(apiKey, secretKey);
            httpClient = new HttpClient();
            this.apiRootPath = apiRootPath;
        }

        public SAuthResponse testConnection() {
            string response = simpleGet(this.apiRootPath + "/API/v1/system/test").GetAwaiter().GetResult();
            SAuthResponse res = JsonConvert.DeserializeObject<SAuthResponse>(response);
            Console.WriteLine("");
            return res;
        }

        public string testWebHookConnection() {
            return simpleGet(this.apiRootPath + "/API/v1/system/testwebhook").GetAwaiter().GetResult();
        }

        public SSaleResponse testSale() {
            string requestUri = this.apiRootPath + "/API/v1/transaction/sale";
            setAuthenticationHeader(requestUri);

            SSaleRequest req = new SSaleRequest(2.0f);
            string json = JsonConvert.SerializeObject(req);

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(requestUri, content).GetAwaiter().GetResult();

            string jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            SSaleResponse SaleResponse = JsonConvert.DeserializeObject<SSaleResponse>(jsonResponse);

            return SaleResponse;
        }

        #region [auxiliary]
        private void setAuthenticationHeader(string url)
        {
            httpClient.DefaultRequestHeaders.Authorization = authGen.getAuthenticationHeaderValue(url);
        }

        private async Task<string> simpleGet(string url) {
            setAuthenticationHeader(url);

            string response = await httpClient.GetStringAsync(url);
            return response;
        }
        #endregion
    }
}
