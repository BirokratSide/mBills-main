using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mBillsTest.api_facade.persistent
{
    public class mBillsDatabase
    {

        CMsSqlConnection conn;
        string db_name = "mBills";
        string table_name = "MBillsTransaction";

        public mBillsDatabase() {
            string db_conn_string = "Data Source=DESKTOP-NBRF35K\\SQLEXPRESS;" +
                                    "Initial Catalog=biroside;" +
                                    "User id=turizem;" +
                                    "Password=q;" +
                                    "Integrated security=False;";
            conn = new CMsSqlConnection(db_conn_string);
            MBillsContext context = new MBillsContext(db_conn_string);

            SMBillsTransaction trans = new SMBillsTransaction() {
                Amount_in_cents = 100,
                Biro_stevilka_racuna = "some",
                Channel_id = "",
                Currency = "EUR",
                MPO1 = "",
                Order_id = "",
                Payment_token_number = "",
                Status = "",
                Transaction_id = "some"
            };
            context.Transactions.Add(trans);
            context.SaveChanges();
        }
    }
}