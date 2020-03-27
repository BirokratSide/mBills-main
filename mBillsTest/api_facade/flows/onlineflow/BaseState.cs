using mBillsTest.api_facade.persistent;
using mBillsTest.structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mBillsTest.api_facade.flows.states
{

    public class BaseState
    {

        IOnlinePaymentFlowState state;
        OnlinePaymentFlow flow;
        
        public IOnlinePaymentFlowState GetCorrespondingState(string status, IOnlinePaymentFlowState state)
        {
            ETransactionStatus s = TransactionStatus.FromString(status);
            return GetCorrespondingState(s, state);
        }

        public IOnlinePaymentFlowState GetCorrespondingState(ETransactionStatus status, IOnlinePaymentFlowState state)
        {
            switch (status)
            {
                case ETransactionStatus.Accepted:
                    return new AcceptedState(state, flow);
                default:
                    return null;
            }
        }

        protected bool RefreshTransaction() {
            try
            {
                ETransactionStatus status = state.api.GetTransactionStatus(state.current_transaction.Transaction_id);
                if (TransactionStatus.FromDatabaseStatus(state.current_transaction.Status) != status) {
                    state.current_transaction.Status = TransactionStatus.ToDatabaseStatus(status);
                    state.database.UpdateTransaction(state.current_transaction);
                    flow.state = GetCorrespondingState(status, state);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected bool VoidTransaction() {
            ETransactionStatus status = state.api.Void(state.current_transaction.Transaction_id, "Your payment reservation has been cancelled!");
            if (status == ETransactionStatus.Voided)
            {
                state.current_transaction.Status = TransactionStatus.ToDatabaseStatus(status);
                state.database.UpdateTransaction(state.current_transaction);
                flow.state = GetCorrespondingState(status, state);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
