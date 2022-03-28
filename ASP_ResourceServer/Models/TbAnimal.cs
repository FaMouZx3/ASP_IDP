using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResourceServer.Models
{
    [Table("tb_animal")]
    public class TbAnimal
    {
        [Column("id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public long id { get; set; }

        [Column("name")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public string name { get; set; }

        [Column("type")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public string type { get; set; }
    }
}
