using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MoneyManager.Models
{
    public partial class Category
    {
        public Category()
        {
            Subcategory = new HashSet<Subcategory>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<Subcategory> Subcategory { get; set; }
    }
}
