using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SerhatKaya.CheckList.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public string UserFullName { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public string Role { get; set; }
        public List<Logs> Logs { get; set; }
        [NotMapped] public string Password { get; set; }
        [NotMapped] public string NewPassword { get; set; }
        [NotMapped] public string Token { get; set; }
    }
}