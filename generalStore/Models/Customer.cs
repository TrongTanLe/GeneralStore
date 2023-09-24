
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace generalStore.Models
{
    public class Customer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }
        [Required]
        public string FullName { get; set; }
        public DateTime Birthday { get; set; }
        public string? Avatar { get; set; }
        public string? Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int? LocationId { get; set; }
        public Location? Location { get; set; }
        public int? District { get; set; }
        public int? Ward { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Password { get; set; }
        public string? Salt { get; set; }
        public DateTime? LastLogin { get; set; }
        [Required]
        public bool? Active { get; set; }
    }
}
