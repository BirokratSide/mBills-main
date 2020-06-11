using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace mBillsTest.api_facade.persistent
{
    [ComVisible(true)]
    [Guid("FEC388C3-B8D6-4CE8-A99C-FFB64E9AF292")] // zgeneriraj unique
    [ClassInterface(ClassInterfaceType.None)]
    public class SMBillsTransaction
    {
        int id;
        string transaction_id;
        string order_id;
        string channel_id;
        string payment_token_number;
        string status;
        int amount_in_cents;
        string currency;
        string MPO;
        string biro_stevilka_racuna;
        DateTime? datetime_started;
        DateTime? datetime_finished;

        public SMBillsTransaction() { }

        [Key]
        public int Id { get => id; set => id = value; }
        [Required]
        public string Transaction_id { get => transaction_id; set => transaction_id = value; }
        public string Order_id { get => order_id; set => order_id = value; }
        public string Channel_id { get => channel_id; set => channel_id = value; }
        public string Payment_token_number { get => payment_token_number; set => payment_token_number = value; }
        public string Status { get => status; set => status = value; }
        [Required]
        public int Amount_in_cents { get => amount_in_cents; set => amount_in_cents = value; }
        [Required]
        public string Currency { get => currency; set => currency = value; }
        public string MPO1 { get => MPO; set => MPO = value; }
        public string Biro_stevilka_racuna { get => biro_stevilka_racuna; set => biro_stevilka_racuna = value; }
        public DateTime? Datetime_started { get => datetime_started; set => datetime_started = value; }
        public DateTime? Datetime_finished { get => datetime_finished; set => datetime_finished = value; }
    }
}
