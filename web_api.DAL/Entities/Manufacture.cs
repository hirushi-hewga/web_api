using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web_api.DAL.Entities
{
    public class Manufacture : BaseEntity<string>
    {
        public override string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }
        [MaxLength(10000)]
        public string? Description { get; set; }
        [MaxLength(255)]
        public string? Founder { get; set; }
        [MaxLength(255)]
        public string? Director { get; set; }
        public string? Image { get; set; }

        public IEnumerable<Car> Cars { get; set; } = [];

    }
}