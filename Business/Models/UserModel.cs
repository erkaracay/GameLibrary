#nullable disable
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models;

public class UserModel
{
    public int Id { get; set; }

    [DisplayName("User Name")]
    [Required(ErrorMessage = "{0} is required!")]
    [MinLength(3, ErrorMessage = "{0} must be minimum {1} characters!")]
    [MaxLength(30, ErrorMessage = "{0} must be maximum {1} characters!")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "{0} is required!")]
    [StringLength(30, MinimumLength = 5, ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
    public string Password { get; set; }

    [DisplayName("Role")]
    public bool IsAdmin { get; set; }

    [DisplayName("Role")]
    public string IsAdminOutput { get; set; }

    [DisplayName("Password")]
    public string PasswordOutput { get; set; }
}

