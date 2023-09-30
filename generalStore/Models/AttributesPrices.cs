using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace generalStore.Models
{
    public class AttributesPrices
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttributesPricesId { get; set; }
        public int AttributeId { get; set; }
        public Attributes? Attribute { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int? Price { get; set; }
        public bool Active { get; set; }
    }
}
