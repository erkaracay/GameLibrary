#nullable disable

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models;

public class GameModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "{0} is required!")]
    [MaxLength(30, ErrorMessage = "{0} must be maximum {1} characters!")]
    public string Name { get; set; }
    public double Price { get; set; }
    public double Revenue { get; set; }
    [DisplayName("Release Date")]
    public DateTime ReleaseDate { get; set; }

    [DisplayName("Release Date")]
    public string ReleaseDateOutput { get; set; }
    [Required(ErrorMessage = "{0} is required!")]
    [DisplayName("Category")]
    public int CategoryId { get; set; }
    [DisplayName("Category")]
    public string CategoryOutput { get; set; }
    [DisplayName("Developer")]
    public int DeveloperId { get; set; }
    [DisplayName("Developer")]
    public string DeveloperOutput { get; set; }
}