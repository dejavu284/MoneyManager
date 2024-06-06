using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MoneyManager.Models
{
    public partial class Currency
    {
        public Currency()
        {
            Account = new HashSet<Account>();
            Deposit = new HashSet<Deposit>();
        }

        public int CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<Account> Account { get; set; }
        public virtual ICollection<Deposit> Deposit { get; set; }
    }
}
