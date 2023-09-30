using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace generalStore.Models
{
    public class TransactStatus
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactStatusId { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
    }
}