using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mBillsTests
{
    class Program
    {
        static void Main(string[] args)
        {
            AuthHeaderGeneratorTests test = new AuthHeaderGeneratorTests("kurac", "kurac", "www.kurac/com/API/v1/jebise");
            test.runTests();
            Console.ReadLine();
        }
    }
}
