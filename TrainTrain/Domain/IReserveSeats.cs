using System.Threading.Tasks;

namespace TrainTrain.Domain
{
    public interface IReserveSeats
    {
        Task<ReservationAttempt> Reserve(string trainId, int seatsRequestedCount);
    }
}