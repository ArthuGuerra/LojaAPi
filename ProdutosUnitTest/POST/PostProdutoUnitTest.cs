using ApiCatalogoUnitTests.Teste;
using ApplicationApi.DataTransferObject;
using ApplicationApi.Services;
using FluentAssertions;
namespace ProdutosUnitTest.POST
{
    public class PostProdutoUnitTest : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutoServices _services;

        public PostProdutoUnitTest(ProdutosUnitTestController controller)
        {
            _services = new ProdutoServices(controller._repo, controller._mapper);
        }

        [Fact]
        public async Task PostProduto_Return_CreateStatusCode()
        {
            // arrange

            var id = Guid.Parse("08dd2862-794e-4773-8b1b-23f57236e6f6");

            var novoProdutoDTO = new ProdutoDTO
            {
                Nome = "ProdutoTeste",
                Estoque = 5,
                Preco = 50,
                CategoriaId = id,
            };

            // act

            var data = await _services.CreateProduto(novoProdutoDTO);

            // assert

            var created = data.Should().BeAssignableTo<ProdutoDTO>();
        }

        [Fact]
        public async Task PostProduto_Return_Badrequest()
        { }
    }
}
