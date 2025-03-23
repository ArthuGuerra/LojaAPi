using ApiCatalogoUnitTests.Teste;
using ApplicationApi.DataTransferObject;
using ApplicationApi.Services;
using FluentAssertions;
using LojinhaApi.Controllers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdutosUnitTest.GET
{
    public class GetProdutosUnitTests : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutoServices _services;

        public GetProdutosUnitTests(ProdutosUnitTestController controller)
        {
            _services = new ProdutoServices(controller._repo, controller._mapper);
        }


        [Fact]
        public async Task GetProdutosById_OkResult()
        {
            // arrange
            
            var prodId = Guid.Parse("08dd2865-5bb5-418d-8f9d-ed0dc709a338");

            // act 

            var data = await _services.GetProdutoID(prodId);
            // Supondo que seja assíncrono

            // Assert

            data.Should().BeAssignableTo<ProdutoDTO>();
            


            // Assert // XUnit
            //var okResult = Assert.IsType<OkObjectResult>(data);
            //Assert.Equal(200, okResult.StatusCode);


            // assert Fluent - Assertion
            //data.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
        }
        [Fact]
        public async Task GetProdutosById_Return_NotFound()
        { }
        [Fact]
        public async Task GetProdutosById_Return_BadRequest()
        { }
        [Fact]
        public async Task GetProdutos_Return_ListOfProdutoDTO()
        {
            // arrange 

            // act
            var prod = await _services.GetProdutos();

            // assert 

            prod.Should().BeAssignableTo<IEnumerable<ProdutoDTO>>().And.NotBeNull();

            
        }
        [Fact]
        public async Task GetProdutos_Return_BadRequestResult()
        { }
      
    }
}
