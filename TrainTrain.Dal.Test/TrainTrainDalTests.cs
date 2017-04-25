using NFluent;
using NUnit.Framework;

namespace TrainTrain.Dal.Test
{
    public class TrainTrainDalTests
    {
        [Test]
        public void Should_add_a_new_train_when_this_one_dont_exist()
        {
            var train = new TrainEntity{ TrainId = "express_2000"};
            train.Seats.Add(new SeatEntity{CoachName = "A", SeatNumber = 1});

            var repository = Factory.Create();
            repository.Save(train);

            Check.That(repository.Get(train.TrainId).TrainId).IsEqualTo(train.TrainId);
        }

        [Test]
        public void Should_remove_all_trains()
        {
            var repository = Factory.Create();
            var trainId = "express_2000";
            repository.RemoveAll();
            Check.That(repository.Get(trainId)).IsNull();
        }
    }
}