using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mBillsTest.api_facade.persistent
{

    public class MBillsContext : DbContext
    {
        public MBillsContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public DbSet<SMBillsTransaction> Transactions { get; set; }
    }
}
