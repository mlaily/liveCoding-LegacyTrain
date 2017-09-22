using System.Collections.Generic;

namespace TrainTrain
{
    public class FailedReservationAttempt : ReservationAttempt
    {
        public FailedReservationAttempt(string trainId, int seatsRequestedCount):base(trainId, seatsRequestedCount, new List<Seat>())
        {
            
        }
    }
}