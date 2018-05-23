using System.Threading.Tasks;
using NFluent;
using NSubstitute;
using NUnit.Framework;

namespace TrainTrain.Test.Acceptance
{
    public class TrainTrainShould
    {
        private const string TrainId = "9043-2018-05-24";
        private const string BookingReference = "75bcd15";

        [Test]
        public async System.Threading.Tasks.Task Reserve_seats_when_train_is_emptyAsync()
        {
            const int seatsRequestedCount = 3;

            var trainDataService = BuildTrainDataService(TrainTopology.With_10_available_seats());
            var bookingReferenceService = BuildBookingReferenceService();

            var webTicketManager = new WebTicketManager(trainDataService, bookingReferenceService);
            var reservation = await webTicketManager.Reserve(TrainId, seatsRequestedCount);

            Check.That(reservation).IsEqualTo($"{{\"train_id\": \"{TrainId}\", \"booking_reference\": \"{BookingReference}\", \"seats\": [\"1A\", \"2A\", \"3A\"]}}");
        }

        [Test]
        public async Task Not_reserve_seats_when_it_exceed_max_capacty_threshold()
        {
            const int seatsRequestedCount = 3;

            var trainDataService = BuildTrainDataService(TrainTopology.With_10_seats_and_6_already_reserved());
            var bookingReferenceService = BuildBookingReferenceService();

            var webTicketManager = new WebTicketManager(trainDataService, bookingReferenceService);
            var reservation = await webTicketManager.Reserve(TrainId, seatsRequestedCount);

            Check.That(reservation).IsEqualTo($"{{\"train_id\": \"{TrainId}\", \"booking_reference\": \"\", \"seats\": []}}");
        }

        [Test]
        [Ignore("While refactoring")]
        public async Task Reserve_all_seats_in_the_same_coach()
        {
            const int seatsRequestedCount = 2;

            var trainDataService = BuildTrainDataService(TrainTopology.With_2_coaches_and_9_seats_already_reserved_in_the_first_coach());
            var bookingReferenceService = BuildBookingReferenceService();

            var webTicketManager = new WebTicketManager(trainDataService, bookingReferenceService);
            var reservation = await webTicketManager.Reserve(TrainId, seatsRequestedCount);

            Check.That(reservation).IsEqualTo($"{{\"train_id\": \"{TrainId}\", \"booking_reference\": \"{BookingReference}\", \"seats\": [\"1B\", \"2B\"]}}");
        }

        private static IBookingReferenceService BuildBookingReferenceService()
        {
            var bookingReferenceService = Substitute.For<IBookingReferenceService>();
            bookingReferenceService.GetBookingReference().Returns(BookingReference);
            return bookingReferenceService;
        }

        private static ITrainDataService BuildTrainDataService(string trainTopology)
        {
            var trainDataService = Substitute.For<ITrainDataService>();
            trainDataService.GetTrain(TrainId).Returns(new Train(TrainDataService.AdaptTrainTopology(trainTopology)));
            return trainDataService;
        }
    }
}
