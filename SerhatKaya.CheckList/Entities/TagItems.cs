using System.Collections.Generic;

namespace SerhatKaya.CheckList.Entities
{
    public class TagItems : BaseEntity
    {
        public long TagId { get; set; }
        public Tag Tag { get; set; }
        public long CategoryId { get; set; }
        public Category Category { get; set; }
    }
}