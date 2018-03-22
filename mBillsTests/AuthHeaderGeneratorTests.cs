using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mBillsTest;

namespace mBillsTests
{
    class AuthHeaderGeneratorTests
    {
        // globals
        AuthHeaderGenerator testedGenerator;
        string apiKey;
        string secretKey;
        string requestUrl;

        public AuthHeaderGeneratorTests(string apiKey, string secretKey, string requestUrl) {
            this.apiKey = apiKey;
            this.secretKey = secretKey;
            this.requestUrl = requestUrl;
            testedGenerator = new AuthHeaderGenerator(apiKey, secretKey);
        }

        public void runTests() {
            testNonce();
            testEpochDateTime();
            testHashedPassword();
        }

        // tests

        // is nonce sensible
        private void testNonce() {
            for (int i = 0; i < 500; i++) {
                string nonce = testedGenerator.getNonce();
                if (!(nonce.Length >= 8 && nonce.Length <= 15)) {
                    Console.WriteLine("NonceTest: the nonce is not of the correct length");
                }
                for (int j = 0; j < nonce.Length; j++) {
                    char curr = nonce[j];
                    if (!(curr >= 48 && curr <= 57)) {
                        Console.WriteLine("NonceTest: the nonce is not all numbers");
                    }
                }
                if (nonce[0] == 48) {
                    Console.WriteLine("NonceTest: the first number of nonce is 0!");
                }
            }
        }

        // is epoch datetime UTC sensible?
        private void testEpochDateTime() {
            // 22.3.2018 11:26 is 1521714363
            if (int.Parse(testedGenerator.getEpochCurrentTimestamp()) < 1521714363) {
                Console.WriteLine("EpochTimeTest: The time is not enough!");
            }
        }

        // is the hashed password hashed into something sensible
        private void testHashedPassword() {
            Console.WriteLine(testedGenerator.getAuthenticationHeaderValue(requestUrl).ToString());
        }

    }
}
