#nullable disable
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models;

public class DeveloperModel
{
	public int Id { get; set; }
    [Required(ErrorMessage = "{0} is required!")]
    public string Name { get; set; }
    [DisplayName("Founding Date")]
	public DateTime FoundingDate { get; set; }
    [DisplayName("Founding Date")]
    public string FoundingDateOutput { get; set; }
}

