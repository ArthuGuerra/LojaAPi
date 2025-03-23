//using Domain.Interfaces;
//using Infraestrutura.Context;
//using Infraestrutura.Repositories;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Infraestrutura.IoC
//{
//    public static class DependencyInjection
//    {
//        public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration config)
//        {
//            services.AddDbContext<LojinhaContext>(options =>
//            options.UseMySql(config.GetConnectionString("DefaultConnection"),
//                new MySqlServerVersion(new Version( ))));

//            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
//            services.AddScoped<IProdutoRepository, ProdutoRepository>();
//            services.AddScoped<ICategoriaServices, CategoriaServices>();
//            services.AddScoped<IProdutoServices, ProdutoServices>();


//            services.AddAutoMapper(typeof(AutoMapperApplication));

//            return services;
//        }
//    }
//}