using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace generalStore.Models
{
    public class Page
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PageId { get; set; }
        public string PageName { get; set; }
        public string? Contents { get; set; }
        public string? Thumb { get; set; }
        [Required]
        public bool Published { get; set; }
        public string? Title { get; set; }
        public string? MetaDesc { get; set; }
        public string? MetaJey { get; set; }
        public string? Alias { get; set; }
        public DateTime CreateDate { get; set; }
        public int? Ordering { get; set; }
    }
}
