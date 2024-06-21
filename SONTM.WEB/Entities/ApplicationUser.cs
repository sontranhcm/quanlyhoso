using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SONTM.WEB.Entities
{
    [Table("ApplicationUser")]
    public class ApplicationUser : BaseEntity
    {
        [Key]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public bool? IsBlock { get; set; }
        [NotMapped]
        public string RoleName { get; set; }
        [NotMapped]
        public string RoleId { get; set; }
        [NotMapped]
        public string Password { get; set; }
    }
}
