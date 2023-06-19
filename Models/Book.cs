using System.ComponentModel.DataAnnotations;

namespace FPTBook.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string? Title { get; set; }
        [Required, Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        [Required, StringLength(500)]
        public string? Description { get; set; }
        public int? AuthorID { get; set; }
        public Author? Author { get; set; }
        public int? PublisherID{ get; set; }
        public Publisher? Publisher { get; set; }
        [Required]
        public string? Poster{get; set;}
        public int CategoryID { get; set; }
        public Category? Category { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        [Required, Range(0, int.MaxValue)]
        public decimal Price { get; set; }
    }
}