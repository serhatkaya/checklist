namespace SerhatKaya.CheckList.Entities
{
    public class CheckListItem : BaseEntity
    {
        public string ItemHeader { get; set; }
        public string ItemText { get; set; }
        public long CheckListId { get; set; }
        public CheckList CheckList { get; set; }
    }
}