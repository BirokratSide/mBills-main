using mBillsTest.api_facade.flows.states;
using mBillsTest.api_facade.persistent;
using mBillsTest.structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mBillsTest.api_facade.flows.onlineflow.states
{
    public class VoidedState : BaseState, IOnlinePaymentFlowState
    {
        public MBillsAPIFacade api { get; set; }
        public mBillsDatabase database { get; set; }
        public SMBillsTransaction current_transaction { get; set; }
        public OnlinePaymentFlow flow;

        public VoidedState(IOnlinePaymentFlowState state, OnlinePaymentFlow flow)
        {
            this.api = state.api;
            this.database = state.database;
            this.current_transaction = null;
            this.flow = flow;
        }


        #region [IOnlinePaymentFlowState]
        public bool StartSale(int amount, string path_to_save_qr)
        {
            return false;
        }

        public bool RefreshCurrentTransaction()
        {
            ETransactionStatus status = api.GetTransactionStatus(current_transaction.Transaction_id);
            if (TransactionStatus.FromDatabaseStatus(current_transaction.Status) != status)
            {
                throw new Exception("The transaction has changed its state from a Voided state. Something is very wrong. Better contact mBills support.");
            }
            return true;
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
            return false;
        }

        public bool ClearCurrentTransaction()
        {
            current_transaction = null;
            flow.state = new EntrypointState(api, database, flow);
            return false;
        }
        #endregion
    }
}
