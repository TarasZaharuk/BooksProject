using BooksProject.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BooksWebApi.Repositories
{
    public class DBManipulator : DbContext
    {
        private IConfiguration _config;

        public DbSet<BookDetailsDto> Books { get; set; }

        public DBManipulator(IConfiguration config) 
        {
            _config = config;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config["DataBasePathSettings:SQLDataBaseConnection"]);
        }

    }
}
