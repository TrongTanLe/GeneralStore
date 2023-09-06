using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace generalStore.Models
{
    public class Account
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }
        [StringLength(12)]
        public string Phone { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        [StringLength(50)]
        public string Password { get; set; }
        [StringLength(6)]
        public string Salt { get; set; }
        [Required]
        public bool Active { get; set; }
        [StringLength(120)]
        public string FullName { get; set; }
        public int RoleId { get; set; }
        public Role? Role { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime CreateDate { get; set;}

    }
}
