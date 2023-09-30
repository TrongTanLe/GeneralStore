using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace generalStore.Models
{
    public class Shipper
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShipperId { get; set; }
        public string? ShipperName { get; set; }
        public string? Phone { get; set; }
        public string? Company { get; set; }
        public DateTime ShipDate { get; set; }
    }
}
