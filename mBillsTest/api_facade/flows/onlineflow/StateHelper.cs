using mBillsTest.api_facade.flows.onlineflow.states;
using mBillsTest.api_facade.persistent;
using mBillsTest.structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mBillsTest.api_facade.flows.states
{

    public static class StateHelper
    {   

        public static IOnlinePaymentFlowState GetCorrespondingState(string status, IOnlinePaymentFlowState state)
        {
            ETransactionStatus s = TransactionStatus.FromString(status);
            return GetCorrespondingState(state, s);
        }

        public static IOnlinePaymentFlowState GetCorrespondingState(IOnlinePaymentFlowState state, ETransactionStatus status)
        {
            switch (status)
            {
                case ETransactionStatus.Accepted:
                    return new AcceptedState(state, state.flow);
                case ETransactionStatus.Authorized:
                    return new AuthorizedState(state, state.flow);
                case ETransactionStatus.InsufficientFunds:
                    return new UnpaidFinishedState(state, state.flow);
                case ETransactionStatus.Paid:
                    return new PaidState(state, state.flow);
                case ETransactionStatus.Pending:
                    throw new Exception("Transaction was in pending state, which is unknown to us at this time.");
                case ETransactionStatus.RecurringCancelled:
                    throw new Exception("Transaction was in RecurringCancelled state. We don't support recurring payments.");
                case ETransactionStatus.Rejected:
                    return new UnpaidFinishedState(state, state.flow);
                case ETransactionStatus.TimeOut:
                    return new UnpaidFinishedState(state, state.flow);
                case ETransactionStatus.TransactionAmountTooBig:
                    return new UnpaidFinishedState(state, state.flow);
                case ETransactionStatus.TransactionAmountTooLow:
                    return new UnpaidFinishedState(state, state.flow);
                default:
                    return null;
            }
        }

        public static bool RefreshTransaction(IOnlinePaymentFlowState state) {
            try
            {
                ETransactionStatus new_status = state.api.GetTransactionStatus(state.current_transaction.Transaction_id);
                ETransactionStatus current_status = TransactionStatus.FromDatabaseStatus(state.current_transaction.Status);
                if (new_status != current_status)
                {
                    PersistAndTransitionState(state, new_status);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool VoidTransaction(IOnlinePaymentFlowState state) {
            ETransactionStatus status = state.api.Void(state.current_transaction.Transaction_id, "Your payment reservation has been cancelled!");
            if (status == ETransactionStatus.Voided)
            {
                PersistAndTransitionState(state, status);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void PersistAndTransitionState(IOnlinePaymentFlowState state, ETransactionStatus status) {
            state.current_transaction.Status = TransactionStatus.ToDatabaseStatus(status);
            state.database.UpdateTransaction(state.current_transaction);
            state.flow.state = GetCorrespondingState(state, status);
        }

        public static void ClearTransaction(IOnlinePaymentFlowState state) {
            state.current_transaction.Datetime_finished = DateTime.Now;
            state.database.UpdateTransaction(state.current_transaction);
            state.current_transaction = null;
            state.flow.state = new EntrypointState(state.api, state.database, state.flow);
        }
    }
}
