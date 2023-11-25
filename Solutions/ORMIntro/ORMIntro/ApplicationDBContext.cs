using Microsoft.EntityFrameworkCore;
using ORMIntro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMIntro
{
    internal class ApplicationDBContext : DbContext
    {
        private const string _connectionString = "Server=.;Database=MinionsDB;User Id=sa;Password=monkeyFlip930!;TrustServerCertificate=True;";

        public DbSet<Town> Towns { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
