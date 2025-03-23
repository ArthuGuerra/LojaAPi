using Domain.Entities;
using Domain.Otimizacao;

namespace Domain.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        public Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(Guid id, ProdutoParameters produtosParameters);
    }
}
