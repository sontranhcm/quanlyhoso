using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SONTM.WEB.Entities
{
    [Table("ApplicationUserRole")]
    public class ApplicationUserRole : BaseEntity
    {
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
    }
}
