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

        string db_name = "mBills";
        string table_name = "MBillsTransaction";
        MBillsContext context;

        public mBillsDatabase()
        {
            string db_conn_string = "Data Source=DESKTOP-NBRF35K\\SQLEXPRESS;" +
                                    "Initial Catalog=biroside;" +
                                    "User id=turizem;" +
                                    "Password=q;" +
                                    "Integrated security=False;";
            MBillsContext context = new MBillsContext(db_conn_string);
        }

        public void InsertRecord(SMBillsTransaction trans)
        {
            context.Transactions.Add(trans);
            context.SaveChanges();
        }

        public void UpdateTransaction(SMBillsTransaction trans)
        {
            SMBillsTransaction origin = context.Transactions.Where(x => x.Transaction_id == trans.Transaction_id).First();
            context.Entry(origin).CurrentValues.SetValues(trans);
            context.SaveChanges();
        }

        public SMBillsTransaction GetLastTransaction()
        {
            if (context.Transactions.Count() > 0)
            {
                return context.Transactions.OrderByDescending(x => x.Id).Take(1).First();
            }
            else
            {
                return null;
            }
        }

        public void demo() {
            SMBillsTransaction trans = new SMBillsTransaction()
            {
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