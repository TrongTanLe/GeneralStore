using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace generalStore.Models
{
    public class Attributes
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttributeId { get; set; }
        public string? Name { get; set; }
    }
}
