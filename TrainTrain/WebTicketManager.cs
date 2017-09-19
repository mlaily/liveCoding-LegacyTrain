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
        
        private readonly ITrainDataService _trainDataService;
        private readonly IBookingReferenceService _bookingReferenceService;

        public WebTicketManager():this(new TrainDataService(UriTrainDataService), new BookingReferenceService(UriBookingReferenceService))
        {

        }

        public WebTicketManager(ITrainDataService trainDataService, IBookingReferenceService bookingReferenceService)
        {

            _trainDataService = trainDataService;
            _bookingReferenceService = bookingReferenceService;
        }
        public async Task<Reservation> Reserve(string trainId, int seatsRequestedCount)
        {
            var train = await _trainDataService.GetTrain(trainId);
            if (train.DoesNotExceedTrainCapacityLimit(seatsRequestedCount))
            {
                var reservationAttempt = train.BuildReservationAttempt(trainId, seatsRequestedCount);

                if (reservationAttempt.IsFulfilled)
                {
                    var bookingRef = await _bookingReferenceService.GetBookingReference();

                    reservationAttempt.AssignBookingReference(bookingRef);
                    
                    await _trainDataService.BookSeats(reservationAttempt);

                    Reservation reservation = reservationAttempt.Confirm();

                    return (reservation);
                }
            }
            return (new ReservationFailed(trainId, seatsRequestedCount));
        }
    }
}