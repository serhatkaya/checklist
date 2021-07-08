using System.Collections.Generic;

namespace SerhatKaya.CheckList.Entities
{
    public class CheckList : BaseEntity
    {
        public long CategoryId { get; set; }
        public Category Category { get; set; }
        public List<Logs> Logs { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        public List<CheckListItem> CheckListItems { get; set; }
        public string Theme { get; set; }
    }
}