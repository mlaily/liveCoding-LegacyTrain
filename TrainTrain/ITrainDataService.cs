using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrainTrain
{
    public interface ITrainDataService
    {
        Task<Train> GetTrain(string trainId);
        Task BookSeats(string trainId, string bookingRef, List<Seat> availableSeats);
    }
}