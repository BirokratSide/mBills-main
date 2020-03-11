using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mBillsTest.structs
{
    public class SSaleRequest
    {
        public int amount; // WARNING!!! THIS IS EXPRESSED IN CENTS AND NOT EUROS!!!
        public string currency;
        public string purpose;
        public string paymentreference; // optional
        public string orderid; // optional
        public string channelid; //optional
        public bool capture; // optional

        public SSaleRequest(int amount_in_cents, string paymentreference = "", string orderid = "", string channelid = "", bool capture = false)
        {
            this.amount = (int)amount_in_cents; 
            this.currency = "EUR";
            this.purpose = "Online payment";
            this.paymentreference = paymentreference;
            this.orderid = orderid;
            this.channelid = channelid;
            this.capture = capture;
        }
    }
}
