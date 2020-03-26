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
using System.Drawing;

namespace mBillsTest
{
    /*
    APICalls's public methods will call the mBills API. At instantiation, it also knows by itself how to set the authorization
    headers for the calls.
    */
    public class MBillsAPIFacade
    {
        string apiRootPath = "";
        MBillsAuthHeaderGenerator authGen;
        MBillsSignatureValidator validator;
        HttpClient httpClient;

        string qrGenPath = "https://qr.mbills.si/qrPng/{0}";


        public MBillsAPIFacade(string apiRootPath, string apiKey, string secretKey, string publicKeyPath) {
            authGen = new MBillsAuthHeaderGenerator(apiKey, secretKey);
            httpClient = new HttpClient();
            validator = new MBillsSignatureValidator(publicKeyPath, apiKey);
            this.apiRootPath = apiRootPath;
        }

        public SAuthResponse testConnection() {
            string response = simpleGet(this.apiRootPath + "/API/v1/system/test").GetAwaiter().GetResult();
            SAuthResponse res = JsonConvert.DeserializeObject<SAuthResponse>(response);
            if (!validator.Verify(res.auth, res.transactionId))
            {
                throw new Exception("Failed to verify MBills response.");
            }
            return res;
        }

        public string testWebHookConnection() {
            return simpleGet(this.apiRootPath + "/API/v1/system/testwebhook").GetAwaiter().GetResult();
        }

        public SSaleResponse testSale(int amountInCents, string documentId = "") {
            string requestUri = this.apiRootPath + "/API/v1/transaction/sale";
            setAuthenticationHeader(requestUri);
            

            SSaleRequest req = new SSaleRequest(amountInCents, orderid: "124134986h", channelid: "eshop1", paymentreference: "SI0015092015");
            if (documentId != "")
                req.documentid = documentId;
            string json = JsonConvert.SerializeObject(req);

            StringContent content = new StringContent(json, System.Text.Encoding.Default, "application/json");
            var response = httpClient.PostAsync(requestUri, content).GetAwaiter().GetResult();
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Status code was bad {response.StatusCode}");

            string jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            SSaleResponse SaleResponse = JsonConvert.DeserializeObject<SSaleResponse>(jsonResponse);

            if (!validator.Verify(SaleResponse.auth, SaleResponse.transactionid)) {
                throw new Exception("Failed to verify MBills response.");
            }

            return SaleResponse;
        }

        // Make Sale method where you input receipt

        public void getQRCode(string tokennumber) {
            
            string addr = string.Format(qrGenPath, tokennumber);
            HttpClient clnt = new HttpClient();
            HttpResponseMessage msg = clnt.GetAsync(addr).GetAwaiter().GetResult();
            Stream srm = msg.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
            Image.FromStream(srm).Save(@"C:\Users\km\Desktop\playground\birokrat\mBills-main\some.jpg");
        }

        public string uploadDocument(string xmlbill)
        {
            // returns documentId
            string requestUri = this.apiRootPath + "/API/v1/document/upload";
            setAuthenticationHeader(requestUri);

            string base64bill = Convert.ToBase64String(Encoding.UTF8.GetBytes(xmlbill));

            string stringContent = $@"
-----BOUNDARY
Content-Disposition: form-data; name=""document[file]""; filename=""racun1.xml""
Content-Type: application/xml
Content-Transfer-Encoding: base64

{base64bill}
-----BOUNDARY--
            ".Trim();
            StringContent content = new StringContent(stringContent, System.Text.Encoding.Default);
            content.Headers.Remove("Content-Type");
            content.Headers.TryAddWithoutValidation("Content-Type", "multipart/form-data; boundary=" + "---BOUNDARY");

            var response = httpClient.PostAsync(requestUri, content).GetAwaiter().GetResult();
            var definition = new { documentId = "" };
            var anon = JsonConvert.DeserializeAnonymousType(response.Content.ReadAsStringAsync().GetAwaiter().GetResult(), definition);
            return anon.documentId;
        }

        public ETransactionStatus getTransactionStatus(string transactionId) {
            string requestUri = this.apiRootPath + $"/API/v1/transaction/{transactionId}/status";
            setAuthenticationHeader(requestUri);

            var response = httpClient.GetAsync(requestUri).GetAwaiter().GetResult();
            var cont = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var anon = new { status = "" };
            string status = JsonConvert.DeserializeAnonymousType(cont, anon).status;
            return TransactionStatus.FromString(status);
        }

        public ETransactionStatus Capture(string transactionid, int amountInCents, string message = "") {
            string requestUri = this.apiRootPath + $"/API/v1/transaction/{transactionid}/capture";
            setAuthenticationHeader(requestUri);

            var anon = new { amount = amountInCents, currency = "EUR", message = message };
            string serialized = JsonConvert.SerializeObject(anon);
            StringContent content = new StringContent(serialized, System.Text.Encoding.Default, "application/json");

            var resp = httpClient.PutAsync(requestUri, content).GetAwaiter().GetResult();
            string con = resp.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var some = new { status = "" };
            string val = JsonConvert.DeserializeAnonymousType(con, some).status;
            return TransactionStatus.FromString(val);
        }

         public ETransactionStatus Void(string transactionid, string message = "") {
            string requestUri = this.apiRootPath + $"/API/v1/transaction/{transactionid}/void";
            setAuthenticationHeader(requestUri);

            var anon = new { message = message };
            string serialized = JsonConvert.SerializeObject(anon);
            StringContent content = new StringContent(serialized, System.Text.Encoding.Default, "application/json");

            var resp = httpClient.PutAsync(requestUri, content).GetAwaiter().GetResult();
            string con = resp.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var some = new { status = "" };
            string val = JsonConvert.DeserializeAnonymousType(con, some).status;
            return TransactionStatus.FromString(val);
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
