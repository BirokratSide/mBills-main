using mBillsTest.api_facade.flows.states;
using mBillsTest.api_facade.persistent;
using mBillsTest.structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mBillsTest.api_facade.flows
{
    public class NullState : BaseState, IOnlinePaymentFlowState
    {
        public MBillsAPIFacade api { get; set; }
        public mBillsDatabase database { get; set; }
        public SMBillsTransaction current_transaction { get; set; }
        public OnlinePaymentFlow flow;

        public NullState(IOnlinePaymentFlowState state, OnlinePaymentFlow flow) {
            this.api = state.api;
            this.database = state.database;
            this.current_transaction = null;
            this.flow = flow;

            current_transaction = database.GetLastTransaction();
            if (current_transaction != null) {
                flow.state = GetCorrespondingState(TransactionStatus.FromDatabaseStatus(current_transaction.Status), state);
            }
        }

        public NullState(MBillsAPIFacade api, mBillsDatabase database, OnlinePaymentFlow flow) {
            this.api = api;
            this.database = database;
            current_transaction = null;
            this.flow = flow;

            current_transaction = database.GetLastTransaction();
            if (current_transaction != null)
            {
                flow.state = GetCorrespondingState(TransactionStatus.FromDatabaseStatus(current_transaction.Status), this);
            }            
        }


        #region [IOnlinePaymentFlowState]
        public bool StartSale(int amount, string path_to_save_qr) {
            // returns the location of the QR code file of the payment
            SSaleResponse resp = api.Sale(amount);
            SMBillsTransaction transaction = new SMBillsTransaction()
            {
                Amount_in_cents = amount,
                Channel_id = resp.channelid,
                Transaction_id = resp.transactionid,
                Currency = "EUR",
                Status = TransactionStatus.ToDatabaseStatus(TransactionStatus.FromInt(resp.status)),
                Order_id = resp.orderid,
                Payment_token_number = resp.paymenttokennumber.ToString(),
            };
            database.InsertRecord(transaction);
            current_transaction = transaction;

            // get qr code and save it to a temp folder
            api.getQRCode(resp.paymenttokennumber.ToString(), path_to_save_qr);

            // change state
            flow.state = GetCorrespondingState(TransactionStatus.FromInt(resp.status), this);
            return true;
        }

        public bool RefreshCurrentTransaction() {
            return base.RefreshTransaction();
        }

        public SMBillsTransaction GetCurrentTransaction() {
            return current_transaction;
        }

        public bool FinishCurrentTransaction(string BiroStevilkaRacuna) {
            return false;
        }

        public bool StornoCurrentTransaction() {
            return false;
        }

        public bool ClearCurrentTransaction() {
            return false;
        }
        #endregion
    }
}
