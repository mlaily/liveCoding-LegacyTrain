using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcmeCorp.TrainDataService.Models
{
    public class Train
    {
        private string trainId;

        public List<Seat> Seats
        {
            get
            {
                var res = new List<Seat>();
                foreach (var coach in this.Coaches)
                {
                    res.AddRange(coach.Seats);
                }

                return res;
            }
        }

        public Train(string trainId)
        {
            this.trainId = trainId;
            this.Coaches = new List<Coach>();
        }

        public List<Coach> Coaches { get; set; }

        public override string ToString()
        {
            var awkwardJson = new StringBuilder("{\"seats\": {");
            var firstElement = true;
            foreach (var seat in this.Seats)
            {
                if (!firstElement)
                {
                    awkwardJson.Append(", ");
                }
                else
                {
                    firstElement = false;
                }

                awkwardJson.Append($"{seat.ToString()}");
            }

            awkwardJson.Append("}}");

            return awkwardJson.ToString();
        }

        public void Add(Coach coach)
        {
            this.Coaches.Add(coach);
        }
    }
}