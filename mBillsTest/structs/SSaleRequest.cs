using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mBillsTest.structs
{
    public class SSaleRequest
    {
        public float amount;
        public string currency;
        public string purpose;
        public string paymentreference; // optional
        public string orderid; // optional
        public string channelid; //optional
        public bool capture; // optional

        public SSaleRequest(float amount, string paymentreference = "", string orderid = "", string channelid = "", bool capture = false)
        {
            this.amount = amount;
            this.currency = "EUR";
            this.purpose = "Online Payment";
            this.paymentreference = paymentreference;
            this.orderid = orderid;
            this.channelid = channelid;
            this.capture = capture;
        }
    }
}
