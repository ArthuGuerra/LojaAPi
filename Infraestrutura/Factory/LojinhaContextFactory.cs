using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Infraestrutura.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

//using Microsoft.Extensions.Configuration.Json;

namespace Infraestrutura.Factories
{
    public class LojinhaContextFactory : IDesignTimeDbContextFactory<LojinhaContext>
    {


        public LojinhaContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LojinhaContext>();

            // Define o caminho do appsettings.json no projeto Api
           

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Aponta para a pasta do projeto LojinhaApi
                .AddJsonFile("C:\\Users\\Arthu\\OneDrive\\Área de Trabalho\\Projetos\\AulaCursoAPI\\LojinhaApi\\LojinhaApi\\appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("ConexaoPadrao");

            if (string.IsNullOrEmpty(connectionString)) throw new Exception("String de conexão não localizada");

            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new LojinhaContext(optionsBuilder.Options);
        }

    }
}
