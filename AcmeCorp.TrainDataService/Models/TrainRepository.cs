using System.Collections.Generic;

namespace AcmeCorp.TrainDataService.Models
{
    public class TrainRepository : IProvideTrain
    {
        readonly Dictionary<string, Train> _trains = new Dictionary<string, Train>();

        public Train GetTrain(string trainId)
        {
            if (!_trains.ContainsKey(trainId))
            {
                // First time, we create the train with default value
                var train = new Train(trainId);
                foreach (var c in "ABCDEFGHIJKL")
                {
                    var coach = new Coach(c.ToString());

                    for (var i = 1; i < 42; i++)
                    {
                        var seat = new Seat(coach.Name, i.ToString(), string.Empty);
                        coach.Seats.Add(seat);
                    }

                    train.Add(coach);
                }

                _trains.Add(trainId, train);
            }

            return _trains[trainId];
        }

        public void UpdateTrainReservations(string jsonFormatForTrainUpdate)
        {
            throw new System.NotImplementedException();
        }
    }

    public class Coach
    {
        public Coach(string coachName)
        {
            this.Name = coachName;
            this.Seats = new List<Seat>();
        }

        public List<Seat> Seats { get; set; }
        public string Name { get; set; }
    }
}