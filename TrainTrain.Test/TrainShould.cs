using NFluent;
using NUnit.Framework;
using TrainTrain.Test.Acceptance;

namespace TrainTrain.Test
{
    class TrainShould
    {
        [Test]
        public void Expose_Coaches()
        {
            var train = new Train("ABCDE", TrainDataService.AdaptTrainToplogy(TrainTopologyGenerator.With_2_coaches_and_9_seats_already_reserved_in_the_first_coach()));
            Check.That(train.Coaches.Values).HasSize(2);
            Check.That(train.Coaches["A"].Seats).HasSize(10);
            Check.That(train.Coaches["B"].Seats).HasSize(10);
        }
    }
}
