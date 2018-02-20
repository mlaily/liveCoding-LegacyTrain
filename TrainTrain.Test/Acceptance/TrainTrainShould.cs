using System.Threading.Tasks;
using NFluent;
using NSubstitute;
using NUnit.Framework;
using TrainTrain.Domain;
using TrainTrain.Infra;

namespace TrainTrain.Test.Acceptance
{
    public class TrainTrainShould
    {
        private const string TrainId = "9043-2018-02-16";
        private const string BookingReference = "75bcd15";

        [Test]
        public async Task Reserve_3_seats_in_empty_train()
        {
            const int seatsRequestedCount = 3;

            // Step1: Instantiate the "I want to go out" adapters
            var trainDataServiceAdapter = BuildTrainDataService(TrainId, TrainTopologyBuilder.GetTrainWith1CoachAnd10AvailableSeats());
            var bookingReferenceServiceAdapter = BuildBookingReferenceService();
            bookingReferenceServiceAdapter.GetBookingReference().Returns(BookingReference);

            // Step2: Instantiate the hexagon
            IReserveSeats hexagon = new SeatsReservation(trainDataServiceAdapter, bookingReferenceServiceAdapter);

            // Step3: Instantiate the "I want to go in" adapter(s)
            var seatsReservationAdapter = new SeatsReservationAdapter(hexagon);
            var jsonReservation = await seatsReservationAdapter.PostReservation(new ReservationRequestDto() { number_of_seats = seatsRequestedCount , train_id = TrainId });

            //var reservation = hexagon.Reserve(TrainId, seatsRequestedCount).Result;

            Check.That(jsonReservation).IsEqualTo($"{{\"train_id\": \"{TrainId}\", \"booking_reference\": \"{BookingReference}\", \"seats\": [\"1A\", \"2A\", \"3A\"]}}");
        }

        [Test]
        public void Not_reserve_when_train_exceed_train_limit_of_seventy_percent()
        {
            const int seatsRequestedCount = 2;

            var trainDataService = BuildTrainDataService(TrainId, TrainTopologyBuilder.GetTrainWith1CoachAnd4AvailableSeats());
            var bookingReferenceService = BuildBookingReferenceService();

            var webTicketManager = new SeatsReservation(trainDataService, bookingReferenceService);
            var reservation = webTicketManager.Reserve(TrainId, seatsRequestedCount).Result;

            Check.That(SeatsReservationAdapter.AdaptReservation(reservation)).IsEqualTo($"{{\"train_id\": \"{TrainId}\", \"booking_reference\": \"\", \"seats\": []}}");
        }

        [Test]
        [Ignore("While refactoring")]
        public void Reserve_all_seats_in_the_same_coach()
        {
            const int seatsRequestedCount = 2;

            var trainDataService = BuildTrainDataService(TrainId, TrainTopologyBuilder.GetTrainWith2CoachesWith9SeatsAlreadyReservedInTheFirstCoach());
            var bookingReferenceService = BuildBookingReferenceService();

            var webTicketManager = new SeatsReservation(trainDataService, bookingReferenceService);
            var reservation = webTicketManager.Reserve(TrainId, seatsRequestedCount).Result;

            Check.That(SeatsReservationAdapter.AdaptReservation(reservation)).IsEqualTo($"{{\"train_id\": \"{TrainId}\", \"booking_reference\": \"{BookingReference}\", \"seats\": [\"1B\", \"2B\"]}}");
        }

        private static ITrainDataService BuildTrainDataService(string trainId, string trainToplogy)
        {
            var trainDataService = Substitute.For<ITrainDataService>();
            trainDataService.GetTrain(trainId).Returns(new Train(TrainDataService.AdaptTrainTopology(trainToplogy)));
            return trainDataService;
        }

        private static IBookingReferenceService BuildBookingReferenceService()
        {
            var bookingReferenceService = Substitute.For<IBookingReferenceService>();
            bookingReferenceService.GetBookingReference().Returns(BookingReference);
            return bookingReferenceService;
        }
    }
}
