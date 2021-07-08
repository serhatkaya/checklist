using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SerhatKaya.CheckList.Entities
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; }
        public List<TagItems> TagItems { get; set; }
        public List<CheckList> CheckLists { get; set; }
        public string Description { get; set; }
        public string Theme { get; set; }
        [NotMapped] public long Count { get; set; }
    }
}