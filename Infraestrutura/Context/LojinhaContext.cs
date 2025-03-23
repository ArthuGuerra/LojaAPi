using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Context
{
    public class LojinhaContext : IdentityDbContext<ApplicationUser>
    {
        public LojinhaContext(DbContextOptions<LojinhaContext> options) :base(options)
        {

        }

        public DbSet<Produto> Produto { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<UserModelDTO> UserModelDTO { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.ApplyConfigurationsFromAssembly(typeof(LojinhaContext).Assembly);
        }

    }
}
