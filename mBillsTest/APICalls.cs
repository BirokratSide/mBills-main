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
    /*
    APICalls's public methods will call the mBills API. At instantiation, it also knows by itself how to set the authorization
    headers for the calls.
    */
    public class APICalls
    {
        string apiRootPath = "";
        AuthHeaderGenerator authGen;
        HttpClient httpClient;


        public APICalls(string apiRootPath, string apiKey, string secretKey) {
            authGen = new AuthHeaderGenerator(apiKey, secretKey);
            httpClient = new HttpClient();
            this.apiRootPath = apiRootPath;
        }

        public string testConnection() {
            return simpleGet(this.apiRootPath + "/API/v1/system/test").GetAwaiter().GetResult();
        }

        public string testWebHookConnection() {
            return simpleGet(this.apiRootPath + "/API/v1/system/testwebhook").GetAwaiter().GetResult();
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
