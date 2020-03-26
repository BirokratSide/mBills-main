using mBillsTest.structs;
using mBillsTests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace mBillsTest.api_facade.security
{
    public class MBillsAuthenticator
    {

        MBillsSignatureValidator validator;
        MBillsAuthHeaderGenerator authGen;
        HttpClient client;

        public MBillsAuthenticator(HttpClient client) {
            string apiKey = GAppSettings.Get("TEST_APIKEY");
            string secretKey = GAppSettings.Get("TEST_SECRETKEY");
            string endpoint = GAppSettings.Get("TEST_ENDPOINT");
            string publicKeyPath = GAppSettings.Get("TEST_PUBLICKEYFILEPATH");
            validator = new MBillsSignatureValidator(publicKeyPath, apiKey);
            authGen = new MBillsAuthHeaderGenerator(apiKey, secretKey);
            this.client = client;
        }

        public string AuthenticateAndVerify(string requestUri, Func<string> requestLogic)
        {
            setAuthenticationHeader(requestUri);
            string returnVal = requestLogic();
            var anon = new { auth = new SAuthInfo(), transactionid = "" };
            var json = JsonConvert.DeserializeAnonymousType(returnVal, anon);

            if (!validator.Verify(json.auth, json.transactionid))
            {
                throw new Exception("Failed to verify MBills response.");
            }
            return returnVal;
        }

        private void setAuthenticationHeader(string url)
        {
            client.DefaultRequestHeaders.Authorization = authGen.getAuthenticationHeaderValue(url);
        }
    }
}
