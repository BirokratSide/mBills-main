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

        static string apiRootPath = "https://private-anon-68d8958991-mbillsquickpaymentapi.apiary-mock.com";
        static string apiKey = "example-apikey";
        static string secretKey = "example-secretkey";
        static string publicKeyFile = "mbills-server-public-key.txt";
        static string workingDirectory = @"C:\Users\Kristijan\Desktop\playground\mBillsTest";
        static APICalls api;

        // logic
        static void Main(string[] args)
        {
            setDirectoryToRootFolder();

            api = new APICalls(apiRootPath, apiKey, secretKey);
            api.testWebHookConnection();

            Console.ReadLine();
        }

        private static void setDirectoryToRootFolder() {
            try
            {
                Directory.SetCurrentDirectory(workingDirectory);
            }
            catch (DirectoryNotFoundException e) {
                
                Console.WriteLine("The specified directory does not exist. {0}", e);
            }
        }

    }
}
