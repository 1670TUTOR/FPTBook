namespace FPTBook.Models;
using System.ComponentModel.DataAnnotations;
public class Author
{
    public int Id { get; set; }
    [Required, StringLength(100)]
    public string Name { get; set; }
    [Required, StringLength(500)]
    public string Description { get; set; }
    public ICollection<Book>? Books { get; set; }
}