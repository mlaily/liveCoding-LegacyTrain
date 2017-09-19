using System.Collections.Generic;

namespace TrainTrain
{
    public class ReservationFailed : ReservationAttempt
    {
        public ReservationFailed(string trainId, int seatsRequestedCount):base(trainId, seatsRequestedCount, new List<Seat>())
        {
        }
    }
}