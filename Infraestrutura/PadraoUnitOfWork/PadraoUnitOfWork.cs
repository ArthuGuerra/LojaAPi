

using Domain.Entities;
using Domain.Interfaces;
using Infraestrutura.Context;
using Infraestrutura.Repositories;

namespace Infraestrutura.PadraoUnitOfWork
{
    public class PadraoUnitOfWork : IUnitOfWork
    {
        private IProdutoRepository _produtoRepository;
        private ICategoriaRepository _categoriaRepository;
        private IUserModelDTO _userModelDTO;

        private readonly LojinhaContext _context;

        public PadraoUnitOfWork(LojinhaContext context)
        {
            _context = context;
        }

        public IProdutoRepository ProdutoRepo 
        { 
            get
            {
                return _produtoRepository = _produtoRepository ?? new ProdutoRepository(_context);
            }              
        }

        public ICategoriaRepository CategoriaRepo
        {
            get 
            {
                return _categoriaRepository = _categoriaRepository ?? new CategoriaRepository(_context);
            }
        }

        public IUserModelDTO UserModelDTO
        {
            get
            {
                return _userModelDTO = _userModelDTO ?? new UserModelDTORepository(_context);
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            } catch(Exception ex)
            {
                throw new Exception("Ocorreu uma exceção. Erro ao salvar no Banco de dados" + (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
            }
        }
    }
}
