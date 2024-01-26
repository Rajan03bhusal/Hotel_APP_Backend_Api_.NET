using Hotel_Reservation.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Reservation.Context
{
    public class MyDbContext:DbContext
    {
        public MyDbContext(DbContextOptions <MyDbContext> options):base(options)
        {

            
        }

        public DbSet <User> Users { get; set; }
        public DbSet <Room> Rooms { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Room>().ToTable("Rooms");

        }
    }


}
