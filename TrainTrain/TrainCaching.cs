using System;
using System.Threading.Tasks;
using TrainTrain.Dal;
using TrainTrain.Dal.Entities;

namespace TrainTrain
{
    public class TrainCaching
    {
        public static async Task Save(string train, Train trainInst, string bookingRef)
        {
            await Task.Run((Action) (() => cache(trainInst, train, bookingRef)));
        }

        public static void Clear()
        {
            Factory.Create().RemoveAll();
        }

        private static void cache(Train trainInst, string trainId, string bookingRef)
        {
            var trainEntity = new TrainEntity { TrainId = trainId };
            foreach (var seat in trainInst.Seats)
            {
                trainEntity.Seats.Add(new SeatEntity { TrainId = trainId, BookingRef = bookingRef, CoachName = seat.CoachName, SeatNumber = seat.SeatNumber });
            }
            Factory.Create().Save(trainEntity);
        }
    }
}