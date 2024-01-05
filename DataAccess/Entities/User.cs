#nullable disable
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class User : Record
    {
        [Required]
        [StringLength(30)]
        public string UserName { get; set; }

        [Required]
        [StringLength(30)]
        public string Password { get; set; }

        public bool IsAdmin { get; set; }
    }
}