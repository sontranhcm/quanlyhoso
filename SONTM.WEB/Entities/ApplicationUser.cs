using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SONTM.WEB.Entities
{
    [Table("ApplicationUser")]
    public class ApplicationUser : BaseEntity
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public string? PasswordHash { get; set; }
        public bool? IsBlock { get; set; }
        [NotMapped]
        public IEnumerable<ApplicationRole> Roles { get; set; }
        [NotMapped]
        [Required]
        public string Password { get; set; }
    }
}
