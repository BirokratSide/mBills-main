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
    internal class UnpaidFinishedState : IOnlinePaymentFlowState
    {
        /*
        Voided, InsufficientFunds, AmountTooBig, AmountTooLow, Rejected, TimeOut will all map to this state
        */

        public MBillsAPIFacade api { get; set; }
        public mBillsDatabase database { get; set; }
        public SMBillsTransaction current_transaction { get; set; }
        public OnlinePaymentFlow flow { get; set; }
        
        public UnpaidFinishedState(IOnlinePaymentFlowState state, OnlinePaymentFlow flow)
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
            return false; // no reason to refresh a finished transaction
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
            StateHelper.ClearTransaction(this);
            return true;
        }
        #endregion
    }
}
