using System.ComponentModel.DataAnnotations;

namespace generalStore.Models.ViewModels
{
    public class CartItem
    {
        [Key]
        public int CartId { get; set; }
        public Product? Product { get; set; }
        public int amount { get; set; }
        public double TotalMoney => (double)(amount * Product.ProductDiscount);
    }
}
