using NFluent;
using NUnit.Framework;
using TrainTrain.Domain;
using TrainTrain.Infra;

namespace TrainTrain.Test.Acceptance
{
    public class TrainShould
    {
        [Test]
        public void Be_a_Value_type()
        {
            var train = new Train(TrainDataService.AdaptTrainTopology(TrainTopologyBuilder.GetTrainWith1CoachAnd4AvailableSeats()));
            var otherTrain = new Train(TrainDataService.AdaptTrainTopology(TrainTopologyBuilder.GetTrainWith1CoachAnd4AvailableSeats()));

            Check.That(otherTrain).IsEqualTo(train);

            var coachA = otherTrain.Coaches["A"];
            coachA.AddSeat(new Seat("A", 99));
            Check.That(otherTrain).IsEqualTo(train);

            // TODO: handle coaches
        }

        [Test]
        public void Expose_Coaches()
        {
            var train = new Train(TrainDataService.AdaptTrainTopology(TrainTopologyBuilder.GetTrainWith2CoachesWith9SeatsAlreadyReservedInTheFirstCoach()));

            Check.That(train.Coaches).HasSize(2);
        }
    }
}