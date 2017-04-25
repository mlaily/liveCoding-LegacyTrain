using System.Data.Entity;

namespace TrainTrain.Dal
{
    public class TrainContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SeatEntity>().HasKey(t => new { t.CoachName, t.SeatNumber });
        }

        public DbSet<TrainEntity> Trains { get; set; }
        public DbSet<SeatEntity> Seats { get; set; }
    }
}