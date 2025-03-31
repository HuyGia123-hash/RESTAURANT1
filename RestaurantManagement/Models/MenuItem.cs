using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string Description { get; set; }
        public string Category { get; set; }
        public bool IsAvailable { get; set; }
    }
    public enum Category
    {
        MonMan,
        MonNhe,
        MonAnVat,
        MonChay,
        NuocUongBia
    }
}
