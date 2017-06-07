using System.Threading.Tasks;
using NFluent;
using NSubstitute;
using NUnit.Framework;

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

            var webTicketManager = new WebTicketManager(trainDataService, bookingReferenceService);
            var reservation = webTicketManager.Reserve(trainId, 3).Result;
            Check.That(reservation).IsEqualTo("{\"train_id\": \"9043-2017-06-07\", \"booking_reference\": \"75bcd15\", \"seats\": [\"1A\", \"2A\", \"3A\"]}");
        }

        [Test]
        public void Should_not_reserve_seats_when_it_exceed_max_capacty_threshold()
        {
            const string trainId = "9043-2017-06-07";
            const string bookingReference = "75bcd15";

            var trainDataService = BuildTrainDataService(trainId, TrainTopologyGenerator.GetTrainWith10ASeatsAnd6AlreadyReserved());
            var bookingReferenceService = BuildBookingReferenceService(bookingReference);

            var webTicketManager = new WebTicketManager(trainDataService, bookingReferenceService);
            var reservation = webTicketManager.Reserve(trainId, 3).Result;
            Check.That(reservation).IsEqualTo("{\"train_id\": \"9043-2017-06-07\", \"booking_reference\": \"\", \"seats\": []}");
        }

        [Test]
        public void Should_reserve_all_seats_in_the_same_coach()
        {
            const string trainId = "9043-2017-06-07";
            const string bookingReference = "75bcd15";

            var trainDataService = BuildTrainDataService(trainId, TrainTopologyGenerator.GetTrainWith2CoachesAnd9SeatsAlreadyReservedInTheFirstCoach());
            var bookingReferenceService = BuildBookingReferenceService(bookingReference);

            var webTicketManager = new WebTicketManager(trainDataService, bookingReferenceService);
            var reservation = webTicketManager.Reserve(trainId, 2).Result;
            Check.That(reservation).IsEqualTo("{\"train_id\": \"9043-2017-06-07\", \"booking_reference\": \"75bcd15\", \"seats\": [\"1B\", \"2B\"]}");
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
