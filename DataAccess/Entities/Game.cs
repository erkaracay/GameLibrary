#nullable disable

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities;

public class Game : Record
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
	public double Price { get; set; }
	public double Revenue { get; set; }
    [Required]
    [DisplayName("Category")]
    public int CategoryId { get; set; }
    [DisplayName("Category")]
    public Category Category { get; set; }
    public int? DeveloperId { get; set; }
    public Developer Developer { get; set; }
    public DateTime ReleaseDate { get; set; }
}