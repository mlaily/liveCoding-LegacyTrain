using System.Collections.Generic;

namespace TrainTrain
{
    public class ReservationFailed : ReservationAttempt
    {
        public ReservationFailed(int seatsRequestedCount):base(seatsRequestedCount, new List<Seat>())
        {
        }
    }
}