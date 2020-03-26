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
using mBillsTest.api_facade.structs;
using mBillsTest.api_facade.security;

namespace mBillsTest
{
    /*
    APICalls's public methods will call the mBills API. At instantiation, it also knows by itself how to set the authorization
    headers for the calls.
    */
    public class MBillsAPIFacade
    {
        string apiRootPath = "";
        HttpClient httpClient;
        MBillsAuthenticator authenticator; 
        string qrGenPath = "https://qr.mbills.si/qrPng/{0}";


        public MBillsAPIFacade(string apiRootPath) {
            httpClient = new HttpClient();
            authenticator = new MBillsAuthenticator(httpClient);
            this.apiRootPath = apiRootPath;
        }

        #region [api methods]
        public SSaleResponse Sale(int amountInCents, string documentId = "") {
            string requestUri = this.apiRootPath + "/API/v1/transaction/sale";
            string result = authenticator.AuthenticateAndVerify(requestUri, () =>
            {
                SSaleRequest req = new SSaleRequest(amountInCents, orderid: "124134986h", channelid: "eshop1", paymentreference: "SI0015092015");
                if (documentId != "")
                    req.documentid = documentId;
                string json = JsonConvert.SerializeObject(req);

                StringContent content = new StringContent(json, System.Text.Encoding.Default, "application/json");
                var response = httpClient.PostAsync(requestUri, content).GetAwaiter().GetResult();
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Status code was bad {response.StatusCode}");

                return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            });
            SSaleResponse SaleResponse = JsonConvert.DeserializeObject<SSaleResponse>(result);
            return SaleResponse;
        }

        public string UploadDocument(string xmlbill)
        {
            // returns documentId
            string requestUri = this.apiRootPath + "/API/v1/document/upload";
            string result = authenticator.AuthenticateAndVerify(requestUri, () =>
            {
                string base64bill = Convert.ToBase64String(Encoding.UTF8.GetBytes(xmlbill));
                StringContent content = XmlBillTemplate.BillToStringContent(base64bill);
                var response = httpClient.PostAsync(requestUri, content).GetAwaiter().GetResult();
                return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            });
            var definition = new { documentId = "" };
            var anon = JsonConvert.DeserializeAnonymousType(result, definition);
            return anon.documentId;
        }

        public ETransactionStatus GetTransactionStatus(string transactionId) {
            string requestUri = this.apiRootPath + $"/API/v1/transaction/{transactionId}/status";
            string result = authenticator.AuthenticateAndVerify(requestUri, () =>
            {
                var response = httpClient.GetAsync(requestUri).GetAwaiter().GetResult();
                var cont = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return cont;
            });
            var anon = new { status = "" };
            string status = JsonConvert.DeserializeAnonymousType(result, anon).status;
            return TransactionStatus.FromString(status);
        }

        public ETransactionStatus Capture(string transactionid, int amountInCents, string message = "") {
            string requestUri = this.apiRootPath + $"/API/v1/transaction/{transactionid}/capture";
            string result = authenticator.AuthenticateAndVerify(requestUri, () =>
            {
                var anon = new { amount = amountInCents, currency = "EUR", message = message };
                string serialized = JsonConvert.SerializeObject(anon);
                StringContent content = new StringContent(serialized, System.Text.Encoding.Default, "application/json");

                var resp = httpClient.PutAsync(requestUri, content).GetAwaiter().GetResult();
                string con = resp.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return con;
            });
            var some = new { status = "" };
            string val = JsonConvert.DeserializeAnonymousType(result, some).status;
            return TransactionStatus.FromString(val);
        }

        public ETransactionStatus Void(string transactionid, string message = "") {
            string requestUri = this.apiRootPath + $"/API/v1/transaction/{transactionid}/void";
            string result = authenticator.AuthenticateAndVerify(requestUri, () =>
            {
                var anon = new { message = message };
                string serialized = JsonConvert.SerializeObject(anon);
                StringContent content = new StringContent(serialized, System.Text.Encoding.Default, "application/json");

                var resp = httpClient.PutAsync(requestUri, content).GetAwaiter().GetResult();
                string con = resp.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return con;
            });
            var some = new { status = "" };
            string val = JsonConvert.DeserializeAnonymousType(result, some).status;
            return TransactionStatus.FromString(val);
        }

        public void getQRCode(string tokennumber)
        {
            string addr = string.Format(qrGenPath, tokennumber);
            HttpClient clnt = new HttpClient();
            HttpResponseMessage msg = clnt.GetAsync(addr).GetAwaiter().GetResult();
            Stream srm = msg.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
            Image.FromStream(srm).Save(@"C:\Users\km\Desktop\playground\birokrat\mBills-main\some.jpg");
        }

        public SAuthResponse testConnection()
        {
            string requestUrl = this.apiRootPath + "/API/v1/system/test";
            string result = authenticator.AuthenticateAndVerify(requestUrl, () =>
            {
                return httpClient.GetStringAsync(requestUrl).GetAwaiter().GetResult();
            });
            return JsonConvert.DeserializeObject<SAuthResponse>(result);
            
        }

        public string testWebHookConnection()
        {
            string requestUrl = this.apiRootPath + "/API/v1/system/test";
            string result = authenticator.AuthenticateAndVerify(requestUrl, () =>
            {
                return httpClient.GetStringAsync(requestUrl).GetAwaiter().GetResult();
            });
            return result;
        }
        #endregion
    }
}
