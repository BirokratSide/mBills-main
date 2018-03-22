using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mBillsTest
{
    public class TransactionStatus
    {
        static int AmountTooLow = -11;
        static int UserLimitExceeded = -10;
        static int AccountError = -6; // user account not found, account has invalid status or multiple accounts exists
        static int TransactionFailed = -5; // system error has occured and the payment failed
        static int InsufficientFunds = -3;
        static int TimeOut = -2;
        static int Rejected = -1;
        static int Accepted = 0;
        static int Authorized = 2;
        static int Paid = 3; // transaction completed and money transfered to your account
        static int Voided = 4; // transaction has been voided from your side and will not be completed

    }
}
