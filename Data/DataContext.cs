using Microsoft.EntityFrameworkCore;
using SnackVote_Backend.Models;

namespace SnackVote_Backend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Menu> Menus { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vote> Votes { get; set; }



    }
}
