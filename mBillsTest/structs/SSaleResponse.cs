
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mBillsTest.structs
{
    public class SSaleResponse
    {
        public string transactionid;
        public int paymenttokennumber;
        public string orderid;
        public string channelid;
        public int status;
        public string statusdescription;
        public SAuthInfo auth;
    }
}
