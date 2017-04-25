using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace TrainTrain.Dal
{
    public class RepositoryEntity : IRepositoryEntity<TrainEntity>
    {
        public TrainEntity Get(string id)
        {
            using (var db = new TrainContext())
            {
                return db.Trains.SingleOrDefault(t => t.TrainId == id);
            }
        }

        public List<TrainEntity> GetAll()
        {
            using (var db = new TrainContext())
            {
                return db.Trains.ToList();
            }
        }

        public void Save(TrainEntity entity)
        {
            if (entity == null) return;

            using (var db = new TrainContext())
            {
                db.Trains.AddOrUpdate(entity);
                db.SaveChanges();
            }
        }

        public void Remove(TrainEntity entity)
        {
            using (var db = new TrainContext())
            {
                var toBeDeleted = GetTrain(db, entity);
                if (toBeDeleted != null)
                {
                    RemoveTrain(db, toBeDeleted);
                    db.SaveChanges();
                }
            }
        }

        private static TrainEntity GetTrain(TrainContext db, TrainEntity target)
        {
            if (target == null) return null;
            return (from train in db.Trains where train.TrainId == target.TrainId select train).FirstOrDefault();
        }

        public void RemoveAll()
        {
            using (var db = new TrainContext())
            {
                var trainEntities = db.Trains.ToList();
                foreach (var train in trainEntities)
                {
                    RemoveTrain(db, train);
                }
                db.SaveChanges();
            }
        }

        private static void RemoveTrain(TrainContext db, TrainEntity target)
        {
            if (target == null) return;

            foreach (var seat in target.Seats.ToList())
            {
                db.Seats.Remove(seat);
            }
            db.Trains.Remove(target);
        }
    }
}