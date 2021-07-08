namespace SerhatKaya.CheckList.Entities
{
    public class Logs : BaseEntity
    {
        public long CheckListId { get; set; }
        public CheckList CheckList { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
    }
}