using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TrainTrain
{
    public class WebTicketManager
    {
        private const string UriBookingReferenceService = "http://localhost:51691/";
        private const string UriTrainDataService = "http://localhost:50680";
        private readonly ITrainCaching _trainCaching;
        private readonly ITrainDataService _trainDataService;
        private readonly IBookingReferenceService _bookingReferenceService;

        public WebTicketManager():this(new TrainDataService(UriTrainDataService), new BookingReferenceService(UriBookingReferenceService))
        {

        }

        public WebTicketManager(ITrainDataService trainDataService, IBookingReferenceService bookingReferenceService)
        {

            _trainDataService = trainDataService;
            _bookingReferenceService = bookingReferenceService;
            _trainCaching = new TrainCaching();
            _trainCaching.Clear();
    
        }
        public async Task<string> Reserve(string trainId, int seatsRequestedCount)
        {
            // get the train
            var train = await _trainDataService.GetTrain(trainId);
            if (train.ReservedSeats + seatsRequestedCount <= Math.Floor(ThreasholdManager.GetMaxRes() * train.MaxSeat))
            {
                List<Seat> availableSeats = new List<Seat>();
                // find seats to reserve
                for (int index = 0, i = 0; index < train.Seats.Count; index++)
                {
                    var each = train.Seats[index];
                    if (each.BookingRef == "")
                    {
                        i++;
                        if (i <= seatsRequestedCount)
                        {
                            availableSeats.Add(each);
                        }
                    }
                }

                if (availableSeats.Count == seatsRequestedCount)
                {
                    var bookingRef = await _bookingReferenceService.GetBookingReference();

                    foreach (var availableSeat in availableSeats)
                    {
                        availableSeat.BookingRef = bookingRef;
                    }
                    await _trainCaching.Save(trainId, train, bookingRef);

                    await _trainDataService.BookSeats(trainId, bookingRef, availableSeats);
                    return
                        $"{{\"train_id\": \"{trainId}\", \"booking_reference\": \"{bookingRef}\", \"seats\": {dumpSeats(availableSeats)}}}";
                }
            }
            return $"{{\"train_id\": \"{trainId}\", \"booking_reference\": \"\", \"seats\": []}}";
        }

        private string dumpSeats(IEnumerable<Seat> seats)
        {
            var sb = new StringBuilder("[");

            var firstTime = true;
            foreach (var seat in seats)
            {
                if (!firstTime)
                {
                    sb.Append(", ");
                }
                else
                {
                    firstTime = false;
                }

                sb.Append(string.Format("\"{0}{1}\"", seat.SeatNumber, seat.CoachName));
            }

            sb.Append("]");

            return sb.ToString();
        }
    }
}