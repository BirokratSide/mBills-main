using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using mBillsTest.api_facade.persistent;

namespace mBillsTest.api_facade.flows.states
{
    [ComVisible(true)]
    [Guid("5A0263A0-8E35-41A3-B401-309FA785A7BE")]
    [ClassInterface(ClassInterfaceType.None)]
    public class OnlinePaymentFlow
    {
        public IOnlinePaymentFlowState state;

        public OnlinePaymentFlow() {
        }

        public void Construct(string sql_conn_string)
        {
            mBillsDatabase database = new mBillsDatabase(sql_conn_string);
            MBillsAPIFacade api = new MBillsAPIFacade();
            state = new flows.EntrypointState(api, database, this);
        }

        #region [IOnlinePaymentFlowState]
        public bool ClearCurrentTransaction()
        {
            return state.ClearCurrentTransaction();
        }
        
        public bool StartSale(int amount, string path_to_qr)
        {
            return state.StartSale(amount, path_to_qr);
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
