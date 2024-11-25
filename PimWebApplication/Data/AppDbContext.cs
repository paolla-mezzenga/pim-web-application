using Microsoft.EntityFrameworkCore;
using PimWebApplication.Models;

namespace PimWebApplication.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Definição das tabelas (DbSet) que correspondem às suas entidades
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Vendas> Vendas { get; set; }
        public DbSet<Producao> Producao { get; set; }
    }
}
