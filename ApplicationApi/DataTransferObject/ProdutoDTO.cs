using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationApi.DataTransferObject
{
    public class ProdutoDTO
    {
        public string? Nome { get; set; }
        public int Estoque { get; set; }
        public double Preco { get; set; }
        public Guid CategoriaId { get; set; }
    }
}
