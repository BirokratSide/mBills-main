using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mBillsTest;
using System.IO;
using mBillsTest;
using mBillsTest.structs;

using Newtonsoft.Json;
using System.Threading;
using mBillsTest.api_facade.persistent;
using mBillsTest.api_facade.flows.states;

namespace mBillsTests
{
    class Program
    {

        static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(GAppSettings.Get("WORKING_DIRECTORY", ""));

            OnlineFlowTest();

        }

        static void OnlineFlowTest() {
            string db_conn_string = "Data Source=DESKTOP-NBRF35K\\SQLEXPRESS;" +
                                    "Initial Catalog=biroside;" +
                                    "User id=turizem;" +
                                    "Password=q;" +
                                    "Integrated security=False;";
            string path_to_qr = "some.jpg";

            OnlinePaymentFlow flow = new OnlinePaymentFlow();
            flow.Construct(db_conn_string);

            if (flow.GetCurrentTransaction() != null) return;

            flow.StartSale(300, path_to_qr);
            while (TransactionStatus.FromDatabaseStatus(flow.GetCurrentTransaction().Status) != ETransactionStatus.Authorized) {
                flow.RefreshCurrentTransaction();
                Thread.Sleep(1000);
            }
            flow.FinishCurrentTransaction("krena stevilka");
            if (TransactionStatus.FromDatabaseStatus(flow.GetCurrentTransaction().Status) == ETransactionStatus.Paid) {
                flow.ClearCurrentTransaction();
            }
        }

        static void ApiWrapperTest() {
            // authenticate to the API
            MBillsAPIFacade api = new MBillsAPIFacade();
            SAuthResponse response = api.testConnection();
            Console.WriteLine("Response transaction ID: {0}", response.transactionId);

            // upload bill and POS sale
            string docid = api.UploadDocument(File.ReadAllText(GAppSettings.Get("RESOURCES_DIRECTORY") + @"\bill.xml"));

            int amount = 100;

            // start a sale
            SSaleResponse resp = api.Sale(100, docid);
            Console.WriteLine(JsonConvert.SerializeObject(resp));

            // qr code
            api.getQRCode(resp.paymenttokennumber.ToString(), "temp.jpg");

            while (true)
            {
                ETransactionStatus status = api.GetTransactionStatus(resp.transactionid);

                if (status == ETransactionStatus.Authorized)
                {
                    Console.WriteLine("");
                    //status = api.Capture(resp.transactionid, amount, "Thank you for shopping with us!");
                    status = api.Void(resp.transactionid, "Sorry, but you stink so we won't do business with you!");
                }
                if (status == ETransactionStatus.Paid)
                {
                    break;
                }
                if (status == ETransactionStatus.Voided)
                {
                    break;
                }


                Thread.Sleep(3000);
            }

            Console.ReadLine();
        }
    }
}
