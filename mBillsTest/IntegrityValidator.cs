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

namespace mBillsTest
{
    /*
    MBills server encrpyts its responses and webhooks using SHA256withRSA (OID = 1.2.840.113549.1.1.11) 
    */
    public class IntegrityValidator
    {
        string publicKeyFile;
        string publicKey;
        string encryptionAlgorithmOid = "1.2.840.113549.1.1.11";

        public IntegrityValidator(string publicKeyFile) {
            this.publicKeyFile = publicKeyFile;
            this.publicKey = File.ReadAllText(publicKeyFile);
        }

        public IntegrityValidator(string publicKeyFile, string publicKey) {
            this.publicKeyFile = publicKeyFile;
            this.publicKey = publicKey;
        }


        /*
        public void decrypt(string message) {
            string publicKey = this.publicKey;
            publicKey = publicKey.Replace("-----BEGIN PUBLIC KEY-----", "");
            publicKey = publicKey.Replace("-----END PUBLIC KEY-----", "");
            publicKey = publicKey.Replace(" ", "");

            Asn1Object obj = Asn1Object.FromByteArray(Convert.FromBase64String(publicKey));
            DerSequence publicKeySequence = (DerSequence)obj;
            DerBitString encodedPublicKey = (DerBitString)publicKeySequence[1];
            DerSequence pubKey = (DerSequence)Asn1Object.FromByteArray(encodedPublicKey.GetBytes());
            DerInteger modulus = (DerInteger)pubKey[0];
            DerInteger exponent = (DerInteger)pubKey[1];
            RsaKeyParameters keyParameters = new RsaKeyParameters(false, modulus.PositiveValue, exponent.PositiveValue);
            RSAParameters parameters = DotNetUtilities.ToRSAParameters(keyParameters);

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(parameters);

            byte[] result = rsa.Decrypt(Convert.FromBase64String(message), false); // should it really be from base64 string?
            Console.WriteLine(System.Text.Encoding.Default.GetString(result));
        }
        */

        /*
        public void decrypt(string message) {
            EnvelopedCms cryptoEngine = new EnvelopedCms(
                    new ContentInfo(Encoding.UTF8.GetBytes(message)),
                    new AlgorithmIdentifier(new System.Security.Cryptography.Oid("1.2.840.113549.1.1.11")));

            // cryptoEngine.Decode(Encoding.ASCII.GetBytes(message));
            // cryptoEngine.Decrypt(privateKey);

            X509Certificate2 cert = new X509Certificate2(this.publicKeyFile);

            cryptoEngine.Decrypt(;

            //PemReader pem = new PemReader(new TextReader());
            //RSACryptoServiceProvider rsa = pem.ReadPemObject()
        }
        */

    }
}
