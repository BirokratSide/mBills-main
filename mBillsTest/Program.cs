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

        static string apiKey = "kurac";
        static string secretKey = "kurac";
        static APICalls api;

        // logic
        static void Main(string[] args)
        {

            api = new APICalls(apiKey, secretKey);
            api.testWebHookConnection();

            Console.ReadLine();
        }

    }
}
