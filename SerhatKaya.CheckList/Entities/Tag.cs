using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SerhatKaya.CheckList.Entities
{
    public class Tag : BaseEntity
    {
        public string TagName { get; set; }
        public List<TagItems> TagItems { get; set; }
    }
}