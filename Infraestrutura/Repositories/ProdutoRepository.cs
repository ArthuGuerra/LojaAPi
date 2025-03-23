using Domain.Entities;
using Domain.Interfaces;
using Domain.Otimizacao;
using Infraestrutura.Context;

namespace Infraestrutura.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(LojinhaContext options ) : base(options) { }

        public async Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(Guid id, ProdutoParameters produtosParameters)
        {
            var prod = await GetAll();

            var produto = prod.Where(c => c.CategoriaID == id)
                .Skip((produtosParameters.PageNumber - 1) * produtosParameters.PageSize)
                .Take(produtosParameters.PageSize).ToList();

            return produto;

        }

       



    }
}
