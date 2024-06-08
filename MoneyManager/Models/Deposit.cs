using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MoneyManager.Models
{
    public partial class Deposit
    {
        public Deposit()
        {
            DepositOperation = new HashSet<DepositOperation>();
        }

        public int DepositId { get; set; }
        public string DepositName { get; set; }
        public decimal DepositAmount { get; set; }
        public decimal InterestRate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CurrencyId { get; set; }
        public bool Status { get; set; }

        public virtual Currency Currency { get; set; }
        public virtual ICollection<DepositOperation> DepositOperation { get; set; }
    }
}
