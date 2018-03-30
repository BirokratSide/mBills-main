using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mBillsTest;
using System.IO;
using mBillsTest;

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

            APICalls api = new APICalls(endpoint, apiKey, secretKey);
            Console.WriteLine(api.testConnection());


            Console.ReadLine();
        }
    }
}
