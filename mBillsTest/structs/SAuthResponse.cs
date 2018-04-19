using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mBillsTest.structs
{
    public class SAuthResponse
    {
        public string transactionId;
        public string orderid;
        public string channelid;
        public int paymenttokennumber;
        public int status;
        public SAuthInfo auth;
    }
}
