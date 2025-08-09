using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web_api.DAL.Entities
{
    public class Car
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [MaxLength(100)]
        public required string Model { get; set; }
        [Required]
        [MaxLength(100)]
        public required string Brand { get; set; }
        [Range(1800, int.MaxValue)]
        public int Year { get; set; }
        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [MaxLength(50)]
        public string? Gearbox { get; set; }
        [MaxLength(50)]
        public string? Color { get; set; }
        public IEnumerable<string>? Images { get; set; }

        [ForeignKey("Manufacture")]
        public string? ManufactureId { get; set; }
        public Manufacture? Manufacture { get; set; }
    }
}