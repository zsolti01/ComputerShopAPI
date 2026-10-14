using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerShopAPI.Models
{
    public class UpdateComputerDTO
    {
        [Column(TypeName = "varchar(30)")]
        public string? Brand { get; set; }

        [Column(TypeName = "varchar(40)")]
        public string? Type { get; set; }

        [Column(TypeName = "float")]
        public double Display { get; set; }
        public int Memory { get; set; }
    }
}
