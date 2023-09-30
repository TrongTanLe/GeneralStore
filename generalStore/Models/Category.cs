using System.ComponentModel.DataAnnotations;

namespace generalStore.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [StringLength(150)]
        public string? CategoryName { get; set; }
        [StringLength(300)]
        public string? CategoryPhoto { get; set; }
        public int CategoryOrder { get; set; }
        public int? ParentId { get; set; }
        public int? Levels { get; set; }
        public string? Title { get; set; }
        public string? Alias { get; set; }
        public string? MetaDesc { get; set; }
        public string? MetaKey { get; set; }
        public bool Published { get; set; }
        public string? Cover { get; set; }
        public string? SchemaMarkup { get; set; }
    }
}
