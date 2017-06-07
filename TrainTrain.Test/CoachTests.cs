using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFluent;
using NUnit.Framework;

namespace TrainTrain.Test
{
    class CoachTests
    {
        [Test]
        public void Should_be_immutable_value_type()
        {
            var coach = new Coach("A");
            var other = new Coach("A");

            Check.That(other).IsEqualTo(coach);

            other.AddSeat(new Seat("A", 1));

            Check.That(other).IsEqualTo(coach);
        }
    }
}
