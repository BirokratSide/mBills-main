using mBillsTest.api_facade.flows.states;
using mBillsTest.api_facade.persistent;

namespace mBillsTest.api_facade.flows
{
    public interface IOnlinePaymentFlowState
    {
        // properties
        MBillsAPIFacade api { get; set; }
        mBillsDatabase database { get; set; }
        SMBillsTransaction current_transaction { get; set; }
        OnlinePaymentFlow flow { get; set; }


        // methods
        bool StartSale(int amount, string path_to_save_qr);
        SMBillsTransaction GetCurrentTransaction();
        bool RefreshCurrentTransaction();
        bool FinishCurrentTransaction(string BiroStevilkaRacuna);
        bool ClearCurrentTransaction();
        bool StornoCurrentTransaction();
    }
}