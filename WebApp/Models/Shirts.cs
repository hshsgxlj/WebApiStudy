using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Models.Validations;

namespace WebApp.Models
{
    public class Shirt
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShirtId { get; set; }
        [Required]
        public string? Brand { set; get; }
        [Required]
        public string? Color { get; set; }
        [Shirt_EnsureCorrect]
        public int? Size { get; set; }
        [Required]
        public string? Gender { get; set; }
        public double Price { get; set; }
    }
}
