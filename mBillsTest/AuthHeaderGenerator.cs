using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net.Http.Headers;

namespace mBillsTest
{
    class AuthHeaderGenerator
    {

        // user variables
        string apiKey;
        string secretKey;

        public AuthHeaderGenerator(string apiKey, string secretKey) {
            this.apiKey = apiKey;
            this.secretKey = secretKey;
        }

        public AuthenticationHeaderValue getAuthenticationHeaderValue(string requestUrl) {
            // mBills' method of authorization is done by the Basic access authentication http standard - you
            // need to encode the username and password by base64 and then send it. It's the simplest method.
            string username = getUsername();
            string password = getPassword(requestUrl);
            string base64encodedUserAndPass = Convert.ToBase64String(
                                                    System.Text.ASCIIEncoding.ASCII.GetBytes(
                                                    string.Format("{0}:{1}", username, password)));
            AuthenticationHeaderValue retVal = new AuthenticationHeaderValue("Basic", base64encodedUserAndPass);
            return retVal;
        }

        private string getUsername() {

            return apiKey + "." + getNonce() + "." + getEpochCurrentTimestamp();
        }

        private string getPassword(string requestUrl) {
            string rawPassword = getUsername() + secretKey + requestUrl;
            return encodePassword(rawPassword);
        }

        private string encodePassword(string password) {
            // SHA256 encoded HEX string, presuming that the password string was ASCII encoded
            SHA256 encrypter = SHA256Managed.Create();
            byte[] hashValue;
            byte[] passwordByes = Encoding.ASCII.GetBytes(password); // not sure if ASCII encoding??
            hashValue = encrypter.ComputeHash(passwordByes);

            // hash needs to be HEX encoded
            string hexHashString = BitConverter.ToString(hashValue).Replace("-", "");

            return hexHashString;
        }

        public string getNonce() {
            // returns a random 8-15 digit number. Is nonce really just a random 8-15 digit number? 
            // Should it be unique? As it is now, it's just a random number
            Random random = new Random();
            int numDigits = random.Next(7, 15);

            IEnumerable<int> nonceNumbers = Enumerable.Range(0, numDigits).Select(x => random.Next(1, 10));
            string nonceString = random.Next(1, 10) + nonceNumbers.Aggregate("", (acc, x) => acc + x);

            return nonceString;
        }

        public string getEpochCurrentTimestamp() {
            DateTime now = TimeUtils.GetNetworkTime();
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((now - epoch).TotalSeconds).ToString();
        }
    }
}
