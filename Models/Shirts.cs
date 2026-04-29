using System.ComponentModel.DataAnnotations;
using WebApiStudy.Models.Validations;

namespace WebApiStudy.Models
{
    public class Shirt
    {
        public int ShirtId { get; set; }
        [Required]
        public string? Brand { set; get; }
        [Required]
        public string? color { get; set; }
        [Shirt_EnsureCorrect]
        public int Size { get; set; }
        [Required]
        public string? Gender { get; set; }
        public double Price { get; set; }
    }
}
