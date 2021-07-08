using System;

namespace SerhatKaya.CheckList.Entities
{
    public class BaseEntity
    {
        public long Id { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public long? CreatedUser { get; set; }
    }
}