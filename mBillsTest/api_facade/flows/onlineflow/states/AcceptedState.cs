using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mBillsTest.api_facade.persistent;
using mBillsTest.structs;

namespace mBillsTest.api_facade.flows.states
{
    class AcceptedState : BaseState, IOnlinePaymentFlowState
    {
        public MBillsAPIFacade api { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public mBillsDatabase database { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public SMBillsTransaction current_transaction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        OnlinePaymentFlow flow;

        public AcceptedState(IOnlinePaymentFlowState state, OnlinePaymentFlow flow) {
            this.api = state.api;
            this.database = state.database;
            this.current_transaction = state.current_transaction;
            this.flow = flow;
        }

        #region [IOnlinePaymentFlowState]
        public bool StartSale(int amount, string path_to_save_qr)
        {
            return false; // cannot start a new sale when one is already in motion
        }

        public bool RefreshCurrentTransaction()
        {
            return base.RefreshTransaction();
        }

        public SMBillsTransaction GetCurrentTransaction()
        {
            return current_transaction;
        }

        public bool FinishCurrentTransaction(string BiroStevilkaRacuna)
        {
            return false; // cannot finish in this state
        }

        public bool StornoCurrentTransaction()
        {
            return VoidTransaction();
        }

        public bool ClearCurrentTransaction()
        {
            return false; // cannot clear a transaction until it is handled
        }
        #endregion
    }
}
