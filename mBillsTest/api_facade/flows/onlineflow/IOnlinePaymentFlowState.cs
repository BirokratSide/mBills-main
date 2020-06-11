using mBillsTest.api_facade.flows.states;
using mBillsTest.api_facade.persistent;
using System.Runtime.InteropServices;

namespace mBillsTest.api_facade.flows
{
    [ComVisible(true)]
    [Guid("6BE9372B-DDC9-4F49-8C78-AA6D086FACA8")] // zgeneriraj unique (vs2015 -> Tools -> Create GUID)
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
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