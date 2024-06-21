using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SONTM.WEB.Entities
{
    [Table("ApplicationRole")]
    public class ApplicationRole : BaseEntity
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public bool IsSelected { get; set; }
    }
}
