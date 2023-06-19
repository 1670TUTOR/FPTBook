namespace FPTBook.Models;
using System.ComponentModel.DataAnnotations;
public class Category
{
    public int Id { get; set; }
    [Required, StringLength(100)]
    public string? Name { get; set; }
    public string? Status { get; set; }
    public ICollection<Book>? Books { get; set; }
    
}