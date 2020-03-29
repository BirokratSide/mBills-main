using mBillsTest.api_facade.persistent;
using mBillsTest.structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mBillsTest.api_facade.flows.states
{
    internal class PaidState : IOnlinePaymentFlowState
    {
        public MBillsAPIFacade api { get; set; }
        public mBillsDatabase database { get; set; }
        public SMBillsTransaction current_transaction { get; set; }
        public OnlinePaymentFlow flow { get; set; }

        public PaidState(IOnlinePaymentFlowState state, OnlinePaymentFlow flow)
        {
            this.api = state.api;
            this.database = state.database;
            this.current_transaction = state.current_transaction;
            this.flow = flow;
        }


        #region [IOnlinePaymentFlowState]
        public bool StartSale(int amount, string path_to_save_qr)
        {
            return false;
        }

        public bool RefreshCurrentTransaction()
        {
            return StateHelper.RefreshTransaction(this);
        }

        public SMBillsTransaction GetCurrentTransaction()
        {
            return current_transaction;
        }

        public bool FinishCurrentTransaction(string BiroStevilkaRacuna)
        {
            return false; // the transaction is already finished
        }

        public bool StornoCurrentTransaction()
        {
            ETransactionStatus status = api.Refund(current_transaction.Transaction_id, current_transaction.Amount_in_cents, "EUR");
            current_transaction.Status = TransactionStatus.ToDatabaseStatus(status);
            database.UpdateTransaction(current_transaction);
            flow.state = StateHelper.GetCorrespondingState(this,status);
            return true;
        }

        public bool ClearCurrentTransaction()
        {
            StateHelper.ClearTransaction(this);
            return false;
        }
        #endregion
    }
}
