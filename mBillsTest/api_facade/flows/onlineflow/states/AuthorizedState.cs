using mBillsTest.api_facade.persistent;
using mBillsTest.structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mBillsTest.api_facade.flows.states
{
    public class AuthorizedState : BaseState, IOnlinePaymentFlowState
    {
        public MBillsAPIFacade api { get; set; }
        public mBillsDatabase database { get; set; }
        public SMBillsTransaction current_transaction { get; set; }
        public OnlinePaymentFlow flow;

        public AuthorizedState(IOnlinePaymentFlowState state, OnlinePaymentFlow flow)
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
            return base.RefreshTransaction();
        }

        public SMBillsTransaction GetCurrentTransaction()
        {
            return current_transaction;
        }

        public bool FinishCurrentTransaction(string BiroStevilkaRacuna)
        {
            ETransactionStatus status = api.Capture(current_transaction.Transaction_id, current_transaction.Amount_in_cents, "Thank you for shopping with us!");
            if (status == ETransactionStatus.Paid)
            {
                current_transaction.Status = TransactionStatus.ToDatabaseStatus(status);
                current_transaction.Biro_stevilka_racuna = BiroStevilkaRacuna;
                database.UpdateTransaction(current_transaction);
                flow.state = GetCorrespondingState(status, this);
                return true;
            }
            else {
                return false;
            }
        }

        public bool StornoCurrentTransaction()
        {
            return false;
        }

        public bool ClearCurrentTransaction()
        {
            return false;
        }
        #endregion
    }
}
