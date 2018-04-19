using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mBillsTest;
using System.IO;
using mBillsTest;
using mBillsTest.structs;

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
            MBillsAPICaller api = new MBillsAPICaller(endpoint, apiKey, secretKey, publicKeyPath);
            SAuthResponse response = api.testConnection();
            Console.WriteLine("Response transaction ID: {0}", response.transactionId);

            // verify the signature
            MBillsSignatureValidator validator = new MBillsSignatureValidator(publicKeyPath, apiKey);
            Console.WriteLine("Validation result: {0}", validator.Verify(response.auth, response.transactionId));

            // start a sale
            api.testSale();

            Console.ReadLine();
        }
    }
}
