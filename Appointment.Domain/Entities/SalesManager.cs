using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Appointment.Domain.Entities
{
    [Table("sales_managers")]
    public class SalesManager
    {
        [Key]
        [Column("id")] 
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("languages")]
        public List<string> Languages { get; set; } = new();

        [Column("products")]
        public List<string> Products { get; set; } = new();

        [Column("customer_ratings")]
        public List<string> CustomerRatings { get; set; } = new();
        public ICollection<Slot> Slots { get; set; } = new List<Slot>();
    }
}
