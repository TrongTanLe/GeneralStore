using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace generalStore.Models
{
    public class Order
    {
        [Key , DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShipDate { get; set; }
        public int TransactStatusId { get; set; }
        public TransactStatus? TransactStatus { get; set; }
        public bool Deleted { get; set; }
        public bool Paid { get; set; }
        public DateTime PaymentDate { get; set; }
        public int PaymentId { get; set; }
        public string Note { get; set; }
    }
}
