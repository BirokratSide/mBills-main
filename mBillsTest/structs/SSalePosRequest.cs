using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mBillsTest.structs
{
    public class SSalePosRequest
    {
        public float amount;
        public string currency;
        public string purpose;
        public string orderid; // optional
        public string channelid; // optional
        public string usertokenid; // mandatory - this is the barcode scanned
        public string documentid; // mandatory - response from call to /document endpoint. You need to send a valid bill specification beforehand.
        public bool capture; // optional

        public SSalePosRequest(float amount, string usertokenid, string documentid, string paymentreference = "", string orderid = "", string channelid = "", bool capture = true)
        {
            this.amount = amount;
            this.usertokenid = usertokenid;
            this.documentid = documentid;
            this.currency = "EUR";
            this.purpose = "Online Payment";
            this.orderid = orderid;
            this.channelid = channelid;
            this.capture = capture;
        }
    }
}
