using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace generalStore.Models
{
    public class OrderDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }    
        public Order? Order { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int OrderNumber { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }  
        public int Total { get; set; }
        public DateTime ShipDate { get; set; }

    }
}
