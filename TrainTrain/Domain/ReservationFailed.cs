using System.Collections.Generic;

namespace TrainTrain.Domain
{
    public class ReservationFailed : ReservationAttempt
    {
        public ReservationFailed(string trainId, int seatsRequestedCount):base(trainId, seatsRequestedCount, new List<Seat>())
        {
        }
    }
}