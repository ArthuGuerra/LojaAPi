using ApiCatalogoUnitTests.Teste;
using ApplicationApi.DataTransferObject;
using ApplicationApi.Services;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdutosUnitTest.PUT
{
    public class PutProdutoUnitTest : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutoServices _services;

        public PutProdutoUnitTest(ProdutosUnitTestController controller)
        {
            _services = new ProdutoServices(controller._repo, controller._mapper);        
        }

        [Fact]
        public async Task PutProduto_Update_ReturnOkresult()
        {
            // arrange

            var id = Guid.Parse("08dd2862-794e-4773-8b1b-23f57236e6f6");

            var prodId = Guid.Parse("08dd2865-5bb5-418d-8f9d-ed0dc709a338");
         

            var prod = new ProdutoDTO()
            {                
                Nome = "ProdutoAtualizadoteste",
                Estoque = 2,
                Preco = 55,
                CategoriaId = id,
            };

            // act

            var data = await _services.UpdateProduto(prodId, prod);

            // assert

            var cre = data.Should().NotBeNull();
            var created = data.Should().BeAssignableTo<ProdutoDTO>();
        }

        [Fact]
        public async Task PutProduto_Update_Return_BadRequest()
        { }
    }
}
