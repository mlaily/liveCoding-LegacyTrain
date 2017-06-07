using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFluent;
using NUnit.Framework;
using TrainTrain.Test.Acceptance;

namespace TrainTrain.Test
{
    class TrainTests
    {
        [Test]
        public void Should_expose_Coaches()
        {
            var trainId = "E3443fg";
            var train = new Train(TrainTopologyGenerator.GetTrainWith2CoachesAnd9SeatsAlreadyReservedInTheFirstCoach(), trainId);

            Check.That(train.Coaches).HasSize(2);
            Check.That(train.Coaches["A"].Seats).HasSize(10);
            Check.That(train.Coaches["B"].Seats).HasSize(10);
        }

        [Test]
        public void Should_be_an_immutable_value_type()
        {
            var trainId = "E3443fg";
            var train = new Train(TrainTopologyGenerator.GetTrainWith2CoachesAnd9SeatsAlreadyReservedInTheFirstCoach(), trainId);
            var other = new Train(TrainTopologyGenerator.GetTrainWith2CoachesAnd9SeatsAlreadyReservedInTheFirstCoach(), trainId);

            Check.That(other).IsEqualTo(train);
        }
    }
}
