using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MoneyManager.Models
{
    public partial class Subcategory
    {
        public Subcategory()
        {
            BankOperation = new HashSet<BankOperation>();
        }

        public int SubcategoryId { get; set; }
        public string SubcategoryName { get; set; }
        public int CategoryId { get; set; }
        public bool Status { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<BankOperation> BankOperation { get; set; }
    }
}
