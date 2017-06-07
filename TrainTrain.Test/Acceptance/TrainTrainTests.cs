using System.Threading.Tasks;
using NFluent;
using NSubstitute;
using NUnit.Framework;
using TrainTrain.Infra;

namespace TrainTrain.Test.Acceptance
{
    public class TrainTrainTests
    {
        [Test]
        public void Should_reserve_seats_when_train_is_empty()
        {
            const string trainId = "9043-2017-06-07";
            const string bookingReference = "75bcd15";

            var trainDataService = BuildTrainDataService(trainId, TrainTopologyGenerator.GetTrainWith10AvailableSeats());
            var bookingReferenceService = BuildBookingReferenceService(bookingReference);

            var hexagon = new Hexagon(trainDataService, bookingReferenceService);

            var reserveSeatsAdapter = new ReserveSeatsRestAdapter(hexagon);

            var jsonResult = reserveSeatsAdapter.Post(new ReservationRequestDto() { number_of_seats = 3, train_id = trainId }).Result;
            
            Check.That(jsonResult).IsEqualTo("{\"train_id\": \"9043-2017-06-07\", \"booking_reference\": \"75bcd15\", \"seats\": [\"1A\", \"2A\", \"3A\"]}");
        }

        [Test]
        public void Should_not_reserve_seats_when_it_exceed_max_capacty_threshold()
        {
            const string trainId = "9043-2017-06-07";
            const string bookingReference = "75bcd15";

            var trainDataService = BuildTrainDataService(trainId, TrainTopologyGenerator.GetTrainWith10ASeatsAnd6AlreadyReserved());
            var bookingReferenceService = BuildBookingReferenceService(bookingReference);

            var hexagon = new Hexagon(trainDataService, bookingReferenceService);

            var reserveSeatsAdapter = new ReserveSeatsRestAdapter(hexagon);

            var jsonResult = reserveSeatsAdapter.Post(new ReservationRequestDto() { number_of_seats = 3, train_id = trainId }).Result;

            Check.That(jsonResult).IsEqualTo("{\"train_id\": \"9043-2017-06-07\", \"booking_reference\": \"\", \"seats\": []}");
        }

        [Test]
        public void Should_reserve_all_seats_in_the_same_coach()
        {
            const string trainId = "9043-2017-06-07";
            const string bookingReference = "75bcd15";

            var trainDataService = BuildTrainDataService(trainId, TrainTopologyGenerator.GetTrainWith2CoachesAnd9SeatsAlreadyReservedInTheFirstCoach());
            var bookingReferenceService = BuildBookingReferenceService(bookingReference);

            var hexagon = new Hexagon(trainDataService, bookingReferenceService);

            var reserveSeatsAdapter = new ReserveSeatsRestAdapter(hexagon);

            var jsonResult = reserveSeatsAdapter.Post(new ReservationRequestDto() { number_of_seats = 2, train_id = trainId }).Result;

            Check.That(jsonResult).IsEqualTo("{\"train_id\": \"9043-2017-06-07\", \"booking_reference\": \"75bcd15\", \"seats\": [\"1B\", \"2B\"]}");
        }

        [Test]
        public void Should_ideally_not_reserve_seats_in_a_coach_if_it_exceed_70_percent_of_its_capacity()
        {
            const string trainId = "9043-2017-06-07";
            const string bookingReference = "75bcd15";

            var trainDataService = BuildTrainDataService(trainId, TrainTopologyGenerator.GetTrainWith3CoachesAnd5SeatsAlreadyReservedInTheFirstCoach());
            var bookingReferenceService = BuildBookingReferenceService(bookingReference);

            var hexagon = new Hexagon(trainDataService, bookingReferenceService);

            var reserveSeatsAdapter = new ReserveSeatsRestAdapter(hexagon);

            var jsonResult = reserveSeatsAdapter.Post(new ReservationRequestDto() { number_of_seats = 3, train_id = trainId }).Result;

            Check.That(jsonResult).IsEqualTo("{\"train_id\": \"9043-2017-06-07\", \"booking_reference\": \"75bcd15\", \"seats\": [\"1B\", \"2B\", \"3B\"]}");
        }

        [Test]
        public void Should_reserve_seats_in_a_coach_even_if_it_exceed_70_percent_of_its_capacity_in_a_non_ideal_case()
        {
            const string trainId = "9043-2017-06-07";
            const string bookingReference = "75bcd15";

            var trainDataService = BuildTrainDataService(trainId, TrainTopologyGenerator.GetTrainWith3CoachesAnd6SeatsThen4Then4AlreadyReserved());
            var bookingReferenceService = BuildBookingReferenceService(bookingReference);

            var hexagon = new Hexagon(trainDataService, bookingReferenceService);

            var reserveSeatsAdapter = new ReserveSeatsRestAdapter(hexagon);

            var jsonResult = reserveSeatsAdapter.Post(new ReservationRequestDto() { number_of_seats = 6, train_id = trainId }).Result;

            Check.That(jsonResult).IsEqualTo("{\"train_id\": \"9043-2017-06-07\", \"booking_reference\": \"75bcd15\", \"seats\": [\"5B\", \"6B\", \"7B\", \"8B\", \"9B\", \"10B\"]}");
        }

        private static IBookingReferenceService BuildBookingReferenceService(string bookingReference)
        {
            var bookingReferenceService = Substitute.For<IBookingReferenceService>();
            bookingReferenceService.GetBookingReference().Returns(Task.FromResult(bookingReference));
            return bookingReferenceService;
        }

        private static ITrainDataService BuildTrainDataService(string trainId, string trainTopology)
        {
            var trainDataService = Substitute.For<ITrainDataService>();
            trainDataService.GetTrain(trainId)
                .Returns(Task.FromResult(trainTopology));
            return trainDataService;
        }
    }
}
