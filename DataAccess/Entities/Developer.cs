#nullable disable
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class Developer: Record
    {
        [Required]
        public string Name { get; set; }
        public List<Game> Games { get; set; }
        public DateTime FoundingDate { get; set; }
    }
}