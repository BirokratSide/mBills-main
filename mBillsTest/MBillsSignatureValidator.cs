using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

using mBillsTest.structs;

namespace mBillsTest
{
    /*
    MBills server encrpyts its responses and webhooks using SHA256withRSA (OID = 1.2.840.113549.1.1.11) 
    */
    public class MBillsSignatureValidator
    {
        #region // vars //
        string publicKeyFile;
        string publicKey;
        string apiKey;
        string encryptionAlgorithmOid = "1.2.840.113549.1.1.11";
        #endregion

        #region // constructors //
        public MBillsSignatureValidator(string publicKeyFile, string apiKey) {
            this.publicKeyFile = publicKeyFile;
            this.publicKey = File.ReadAllText(publicKeyFile);
            this.apiKey = apiKey;
        }

        public MBillsSignatureValidator(string publicKeyFile, string publicKey, string apiKey)
        {
            this.publicKeyFile = publicKeyFile;
            this.publicKey = publicKey;
            this.apiKey = apiKey;
        }
        #endregion

        #region // public //
        public bool Verify(SAuthInfo response, string itemId) {
            RSACryptoServiceProvider csp = retrieveCryptoServiceProvider();
            string verificationMessage = getVerificationMessage(response, itemId);
            byte[] hash = getSha256Hash(verificationMessage);
            byte[] signature = HexStringToByteArray(response.signature);
            return csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA256"), signature);
        }
        #endregion

        #region // auxiliary //
        private RSACryptoServiceProvider retrieveCryptoServiceProvider() {
            X509Certificate2 cert = new X509Certificate2(this.publicKeyFile);
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)cert.PublicKey.Key;
            return csp;
        }

        // @MBills documentation
        private string getVerificationMessage(SAuthInfo response, string itemId) {
            return this.apiKey + response.nonce + response.timestamp + itemId; // itemId is e.g. transactionId
        }

        private byte[] getSha256Hash(string message) {
            SHA256Managed sha256 = new SHA256Managed();
            byte[] data = Encoding.UTF8.GetBytes(message);
            byte[] hash = sha256.ComputeHash(data);
            return hash;
        }

        public static byte[] HexStringToByteArray(string hex) {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        #endregion
    }
}
