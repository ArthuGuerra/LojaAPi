
namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IProdutoRepository ProdutoRepo { get; }
        ICategoriaRepository CategoriaRepo {  get; }
        IUserModelDTO UserModelDTO { get; }


        Task CommitAsync();

    }
}
