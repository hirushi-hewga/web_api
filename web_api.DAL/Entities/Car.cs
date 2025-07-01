using System.ComponentModel.DataAnnotations;

namespace web_api.DAL.Entities
{
    public class Car
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [MaxLength(100)]
        public required string Model { get; set; }
        [Required]
        [MaxLength(100)]
        public required string Brand { get; set; }
        public int Year { get; set; }
    }
}