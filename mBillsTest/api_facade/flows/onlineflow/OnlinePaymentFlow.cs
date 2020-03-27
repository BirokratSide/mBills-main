using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mBillsTest.api_facade.persistent;

namespace mBillsTest.api_facade.flows.states
{
    public class OnlinePaymentFlow
    {
        public IOnlinePaymentFlowState state;

        public OnlinePaymentFlow() {
        }

        public void Construct()
        {
            mBillsDatabase database = new mBillsDatabase();
            MBillsAPIFacade api = new MBillsAPIFacade();
            state = new flows.NullState(api, database, null, this);
        }

        #region [IOnlinePaymentFlowState]
        public bool ClearCurrentTransaction()
        {
            return state.ClearCurrentTransaction();
        }
        
        public void StartSale()
        {
            state.StartSale();
        }

        public SMBillsTransaction GetCurrentTransaction()
        {
            return state.GetCurrentTransaction();
        }

        public bool RefreshCurrentTransaction()
        {
            return state.RefreshCurrentTransaction();
        }

        public bool FinishCurrentTransaction(string BiroStevilkaRacuna)
        {
            return state.FinishCurrentTransaction(BiroStevilkaRacuna);
        }
        
        public bool StornoCurrentTransaction()
        {
            return state.StornoCurrentTransaction();
        }
        #endregion
    }
}
