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
    internal class EntrypointState : IOnlinePaymentFlowState
    {
        public MBillsAPIFacade api { get; set; }
        public mBillsDatabase database { get; set; }
        public SMBillsTransaction current_transaction { get; set; }
        public OnlinePaymentFlow flow { get; set; }

        public EntrypointState(IOnlinePaymentFlowState state, OnlinePaymentFlow flow)
        {
            this.api = state.api;
            this.database = state.database;
            this.current_transaction = null;
            this.flow = flow;
            LoadLastTransaction(flow);
        }
        public EntrypointState(MBillsAPIFacade api, mBillsDatabase database, OnlinePaymentFlow flow) {
            this.api = api;
            this.database = database;
            current_transaction = null;
            this.flow = flow;
            LoadLastTransaction(flow);
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
            flow.state = StateHelper.GetCorrespondingState(this, TransactionStatus.FromInt(resp.status));
            return true;
        }

        public bool RefreshCurrentTransaction() {
            return StateHelper.RefreshTransaction(this);
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

        #region [auxiliary]
        private void LoadLastTransaction(OnlinePaymentFlow flow)
        {
            SMBillsTransaction last_transaction = database.GetLastTransaction();
            if (last_transaction != null && last_transaction.Datetime_finished != null) {
                current_transaction = last_transaction;
                ETransactionStatus status = TransactionStatus.FromDatabaseStatus(current_transaction.Status);
                flow.state = StateHelper.GetCorrespondingState(this, status);
            }
        }
        #endregion
    }
}
