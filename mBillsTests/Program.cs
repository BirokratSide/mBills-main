using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mBillsTest;
using System.IO;
using mBillsTest;
using mBillsTest.structs;

using Newtonsoft.Json;
using System.Threading;

namespace mBillsTests
{
    class Program
    {

        static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(GAppSettings.Get("WORKING_DIRECTORY", ""));
            string apiKey = GAppSettings.Get("TEST_APIKEY");
            string secretKey = GAppSettings.Get("TEST_SECRETKEY");
            string endpoint = GAppSettings.Get("TEST_ENDPOINT");
            string publicKeyPath = GAppSettings.Get("TEST_PUBLICKEYFILEPATH");

            // authenticate to the API
            MBillsAPIFacade api = new MBillsAPIFacade(endpoint, apiKey, secretKey, publicKeyPath);
            SAuthResponse response = api.testConnection();
            Console.WriteLine("Response transaction ID: {0}", response.transactionId);

            // verify the signature
            MBillsSignatureValidator validator = new MBillsSignatureValidator(publicKeyPath, apiKey);
            Console.WriteLine("Validation result: {0}", validator.Verify(response.auth, response.transactionId));

            // upload bill and POS sale
            string docid = api.uploadDocument(File.ReadAllText(GAppSettings.Get("RESOURCES_DIRECTORY") + @"\bill.xml"));

            int amount = 100;

            // start a sale
            SSaleResponse resp = api.testSale(100, docid);
            Console.WriteLine(JsonConvert.SerializeObject(resp));

            // qr code
            api.getQRCode(resp.paymenttokennumber.ToString());

            while (true) {
                ETransactionStatus status = api.getTransactionStatus(resp.transactionid);

                if (status == ETransactionStatus.Authorized) {
                    Console.WriteLine("");
                    //status = api.Capture(resp.transactionid, amount, "Thank you for shopping with us!");
                    status = api.Void(resp.transactionid, "Sorry, but you stink so we won't do business with you!");
                }
                if (status == ETransactionStatus.Paid) {
                    break;
                }
                if (status == ETransactionStatus.Voided) {
                    break;
                }

                
                Thread.Sleep(3000);
            }

            Console.ReadLine();
        }
    }
}
