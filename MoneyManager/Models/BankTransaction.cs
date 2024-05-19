using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MoneyManager.Models
{
    public partial class BankTransaction
    {
        public int TransactionId { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public int AccountId { get; set; }
        public string TransactionType { get; set; }
        public int SubcategoryId { get; set; }

        public virtual Account Account { get; set; }
        public virtual Subcategory Subcategory { get; set; }
    }
}
