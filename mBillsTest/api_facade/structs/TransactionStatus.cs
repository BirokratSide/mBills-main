using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mBillsTest.structs
{
    public class TransactionStatus
    {
        public static ETransactionStatus FromString(string a) {
            return (ETransactionStatus)int.Parse(a);
        }

        public static ETransactionStatus FromInt(int a)
        {
            return (ETransactionStatus)a;
        }

        public static string ToDatabaseStatus(ETransactionStatus status) {
            return Enum.GetName(typeof(ETransactionStatus), status);
        }

        public static ETransactionStatus FromDatabaseStatus(string status) {
            return (ETransactionStatus) Enum.Parse(typeof(ETransactionStatus), status);
        }
    }

    public enum ETransactionStatus {
        // self explanatory
        Accepted = 0,
        // used in cases where an invoice with a due date was sent to the user, but the user has not paid it yet.
        Pending = 1, 
        // user had confirmed the payment and the amount has been reserved. To receive the money in your wallet you still need
        // to do a capture of the amount or void the authorisation and return the money to the user
        Authorized = 2, 
        // you will receive this status code once the transaction has been completed and money transfered to your wallet account
        Paid = 3,
        // Transaction has been voided from your side and will not be completed.
        Voided = 4,
        // Payment has been rejected by the user.
        Rejected = -1,
        // Payment has timed out and will not be processed.
        TimeOut = -2,
        // Payment has been rejected due to insufficient funds in the user wallet.
        InsufficientFunds = -3,
        // Payment has been rejected and the recurring authorization has been cancelled as well. 
        // You will no longer be able to make recurring payments with this transctionid.
        RecurringCancelled = -4,
        // A system error has occured and the payment failed.
        TransactionFailed = -5,
        // Transaction amount too low.
        TransactionAmountTooLow = -11,
        // Transaction amount too big.
        TransactionAmountTooBig = -12
    }
}
