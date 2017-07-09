using System.Threading.Tasks;
using TrainTrain.Domain;

namespace TrainTrain
{
    public class Hexagon : IReserveSeats
    {
        private IReserveSeats webTicketManager;

        public Hexagon(ITrainDataService trainDataService, IBookingReferenceService bookingReferenceService)
        {
            this.webTicketManager = new TrainReservationService(trainDataService, bookingReferenceService);
        }

        public Task<ReservationAttempt> Reserve(string trainId, int seatsRequestedCount)
        {
            return this.webTicketManager.Reserve(trainId, seatsRequestedCount);
        }
    }
}