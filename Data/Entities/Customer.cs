using System.Collections;
using System.Collections.Generic;

namespace Data.Entities
{
    public class Customer : BaseEntity
    {
        public int PersonId { get; set; }
        public int DiscountValue { get; set; }

        public Person Person { get; set; }
        public ICollection<Receipt> Receipts { get; set; }
    }
}