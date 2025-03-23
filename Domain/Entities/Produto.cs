
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Produto
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Nome { get; set; }

        [StringLength(100)]
        public string? Descricao { get; set; }

        public int Estoque { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public double Preco { get; set; }
        public DateTime DataCadastro { get; set; }

        public Guid CategoriaID { get; set; }

        [JsonIgnore]
        public Categoria Categoria { get; set; }

    }
}
