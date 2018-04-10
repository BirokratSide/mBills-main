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
        public bool VerifyTest(SMBillsAuthResponse response) {

            string verificationMessage = getVerificationMessage(response);
            byte[] hash = getSha256Hash(verificationMessage);
            byte[] signature = Convert.FromBase64String(response.auth.signature); // hmmm

            EnvelopedCms cryptoEngine = new EnvelopedCms(
                    new ContentInfo(hash),
                    new AlgorithmIdentifier(new System.Security.Cryptography.Oid("1.2.840.113549.1.1.11")));
            return true;
        }

        public bool Verify(SMBillsAuthResponse response) {

            RSACryptoServiceProvider csp = retrieveCryptoServiceProvider();
            string verificationMessage = getVerificationMessage(response);
            byte[] hash = getSha256Hash(verificationMessage);
            byte[] signature = StringToByteArray(response.auth.signature); // hmmm
            return csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA256"), signature);
        }
        #endregion


        #region // auxiliary //
        private RSACryptoServiceProvider retrieveCryptoServiceProvider() {
            X509Certificate2 cert = new X509Certificate2(this.publicKeyFile);

            /*RSACryptoServiceProvider csp = (RSACryptoServiceProvider)cert.PublicKey.Key;
            var enhCsp = new RSACryptoServiceProvider().CspKeyContainerInfo;
            string mKeyContainerName = csp.CspKeyContainerInfo.KeyContainerName;
            CspParameters cspparams = new CspParameters(enhCsp.ProviderType, enhCsp.ProviderName, mKeyContainerName);
            csp = new RSACryptoServiceProvider(cspparams);*/
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)cert.PublicKey.Key;
            return csp;
        }

        // @MBills documentation
        private string getVerificationMessage(SMBillsAuthResponse response) {
            return this.apiKey + response.auth.nonce + response.auth.timestamp + response.transactionId;
        }

        private byte[] getSha256Hash(string message) {
            SHA256Managed sha256 = new SHA256Managed();
            byte[] data = Encoding.Unicode.GetBytes(message);
            byte[] hash = sha256.ComputeHash(data);
            return hash;
        }

        public static byte[] StringToByteArray(string hex) {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        #endregion
    }
}
