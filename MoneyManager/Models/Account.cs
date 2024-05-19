using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MoneyManager.Models
{
    public partial class Account
    {
        public Account()
        {
            BankTransaction = new HashSet<BankTransaction>();
            DepositOperation = new HashSet<DepositOperation>();
        }

        public int AccountId { get; set; }
        public decimal AccountBalance { get; set; }
        public int CurrencyId { get; set; }

        public virtual Currency Currency { get; set; }
        public virtual ICollection<BankTransaction> BankTransaction { get; set; }
        public virtual ICollection<DepositOperation> DepositOperation { get; set; }
    }
}
