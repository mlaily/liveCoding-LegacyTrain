using System.Linq;
using System.Threading.Tasks;
using NFluent;
using NSubstitute;
using TechTalk.SpecFlow;
using TrainTrain.Test.Acceptance;

namespace TrainTrain.Spec.StepDefinitions
{
    [Binding]
    public class TrainTrainReservationSteps
    {
        private const string ArbitraryBookingReference = "XYZ0123";
        private string _trainId;
        private int _seatsRequestedCount;

        private string _trainTopology;

        [Given(@"the train ""(.*)""")]
        public void GivenTheTrain(string trainId)
        {
            _trainId = trainId;
        }

        [Given(@"(.*) coaches of (.*) seats available")]
        public void GivenTheTrainContainsCoachesOfSeatsAvailables(int coachCount, int seatsCount)
        {

            _trainTopology = TrainTopologyGenerator.Generate(coachCount, seatsCount);
        }

        [When(@"(.*) seats are requested")]
        public void WhenSeatsRequested(int seatsRequestedCount)
        {
            _seatsRequestedCount = seatsRequestedCount;
        }

        [Given(@"(.*) coaches of (.*) seats and (.*) already reserved")]
        public void GivenCoachesOfSeatsWithAlreadyReserved(int coachesCount, int availableSeatsCount,
            int seatsReservedCount)
        {
            _trainTopology =
                TrainTopologyGenerator.Generate(coachesCount, availableSeatsCount, ArbitraryBookingReference, seatsReservedCount);
        }

        [Given(@"(.*) coaches of (.*) seats and (.*) seats already reserved in the coach (.*)")]
        public void GivenCoachesOfSeatsWithSeatsAlreadyReservedInTheCoach(int coachesCount, int availableSeatsCount, int seatsReservedCount, int coachNumberWhereReserved)
        {
            _trainTopology =
                TrainTopologyGenerator.Generate(coachesCount, availableSeatsCount, ArbitraryBookingReference, seatsReservedCount, coachNumberWhereReserved);
        }

        [Then(@"the reservation (.*) should be assigned these seats ""(.*)""")]
        public void ThenTheReservationShouldReserveSeats(string bookingReference, string expectedSeats)
        {
            var trainDataService = BuildTrainDataService(_trainId, _trainTopology);
            var bookingReferenceService = BuildBookingReferenceService(bookingReference);

            var webTicketManager = new WebTicketManager(trainDataService, bookingReferenceService);
            var jsonReservation = webTicketManager.Reserve(_trainId, _seatsRequestedCount).Result;

            Check.That(jsonReservation)
                .IsEqualTo($"{{\"train_id\": \"{_trainId}\", \"booking_reference\": \"{bookingReference}\", \"seats\": [{AdaptSeatsInJson(expectedSeats)}]}}");         
        }

        [Then(@"the reservation (.*) should be failed")]
        public void ThenTheReservationShouldFail(string bookingReference)
        {
            var trainDataService = BuildTrainDataService(_trainId, _trainTopology);
            var bookingReferenceService = BuildBookingReferenceService(bookingReference);

            var webTicketManager = new WebTicketManager(trainDataService, bookingReferenceService);
            var jsonReservation = webTicketManager.Reserve(_trainId, _seatsRequestedCount).Result;

            Check.That(jsonReservation)
                .IsEqualTo($"{{\"train_id\": \"{_trainId}\", \"booking_reference\": \"\", \"seats\": []}}");
        }
        
        [Then(@"the reservation (.*) should be assigned these seats ""(.*)"" in the same coach")]
        public void ThenTheReservationBcdShouldReserveInSameCoach(string bookingReference, string expectedSeats)
        {
            var trainDataService = BuildTrainDataService(_trainId, _trainTopology);
            var bookingReferenceService = BuildBookingReferenceService(bookingReference);

            var webTicketManager = new WebTicketManager(trainDataService, bookingReferenceService);
            var jsonReservation = webTicketManager.Reserve(_trainId, _seatsRequestedCount).Result;

            Check.That(jsonReservation)
                .IsEqualTo($"{{\"train_id\": \"{_trainId}\", \"booking_reference\": \"{bookingReference}\", \"seats\": [{AdaptSeatsInJson(expectedSeats)}]}}");
        }

        private static string AdaptSeatsInJson(string expectedSeats)
        {
            var expectedSeatsSplited = expectedSeats.Split(',').Select(x => x.Trim());
            return "\"" + string.Join("\", \"", expectedSeatsSplited) + "\"";
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
                .Returns(Task.FromResult(new Train(TrainDataService.AdaptTrainTopology(trainTopology))));
            return trainDataService;
        }
    }
}
