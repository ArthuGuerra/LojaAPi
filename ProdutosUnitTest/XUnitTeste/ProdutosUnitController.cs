using ApplicationApi.DataTransferObject;
using ApplicationApi.Services;
using AutoMapper;
using Domain.Interfaces;
using Domain.Otimizacao;
using FluentAssertions.Common;
using Infraestrutura.Context;
using Infraestrutura.PadraoUnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoUnitTests.Teste
{
    public class ProdutosUnitTestController
    {

        public IUnitOfWork _repo;
        public IMapper _mapper;
        public static DbContextOptions<LojinhaContext> dbContext { get; }


        public static string connectionString = "Server=localhost;Port=3306;Database=LojinhaDb;Uid=root;Pwd=Guerra123@admin";

        static ProdutosUnitTestController()
        {
            dbContext = new DbContextOptionsBuilder<LojinhaContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)).Options;
        }


        public ProdutosUnitTestController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile( new AutoMapperApplication());
            });

            _mapper = config.CreateMapper();
            var context = new LojinhaContext(dbContext);
            _repo = new PadraoUnitOfWork(context);
        }



    }
}
