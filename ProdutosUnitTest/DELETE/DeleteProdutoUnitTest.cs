using ApiCatalogoUnitTests.Teste;
using ApplicationApi.DataTransferObject;
using ApplicationApi.Services;
using Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdutosUnitTest.DELETE
{
    public class DeleteProdutoUnitTest : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutoServices _services;

        public DeleteProdutoUnitTest(ProdutosUnitTestController controller)
        {
            _services = new ProdutoServices(controller._repo, controller._mapper);
        }

        [Fact]
        public async Task DeleteProdutoById_Return_OkResult()
        {
            // arrange

            var id = Guid.Parse("08dd2862-794e-4773-8b1b-23f57236e6f6");
            var prodId = Guid.Parse("08dd452d-9f94-49d1-8e62-d2c5db7b65b7");

            var prod = new ProdutoDTO
            {            
                Nome = "ProdutoDeletado",
                Estoque = 5,
                Preco = 10,
                CategoriaId = id,
            };

            // act 

            var data = await _services.DeleteProduto(prodId);


            // assert

            data.Should().BeAssignableTo<ProdutoDTO>();
        }
    }
}
