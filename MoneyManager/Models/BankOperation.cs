using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MoneyManager.Models
{
    public partial class BankOperation
    {
        public int OperationId { get; set; }
        public decimal OperationAmount { get; set; }
        public DateTime OperationDate { get; set; }
        public string Description { get; set; }
        public int AccountId { get; set; }
        public string OperationType { get; set; }
        public int SubcategoryId { get; set; }

        public virtual Account Account { get; set; }
        public virtual Subcategory Subcategory { get; set; }
    }
}
