using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace generalStore.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        [StringLength(150)]
        public string? ProductName { get; set; }
        [StringLength(3000)]
        public string? ProductDescription { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        [Column(TypeName ="decimal(8,2)")]
        public decimal? ProductPrice { get; set; }
        [Column(TypeName = "decimal(2,2)")]
        public decimal? ProductDiscount { get; set; }
        [StringLength(300)]
        public string? ProductPhoto { get; set; }
        [ForeignKey("Size")]
        public int SizeId { get; set; }
        public Size? Size { get; set; }
        [ForeignKey("Color")]
        public int ColorId { get; set; }
        public Color? Color { get; set; }
        public bool IsTrandy { get; set; }
        public bool IsArrived { get; set; }

    }
}
