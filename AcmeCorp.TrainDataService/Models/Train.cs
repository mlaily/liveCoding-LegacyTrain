using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace AcmeCorp.TrainDataService.Models
{
    public class Train
    {
        private string trainId;
        private readonly List<Seat> _seats;


        public Train(string trainId)
        {
            this.trainId = trainId;
            _seats = new List<Seat>();
            _seats.Add(new Seat("A", "1", ""));
            _seats.Add(new Seat("A", "2", ""));
        }

        public IEnumerable<Seat> seats => _seats;

        public override string ToString()
        {
            var awkwardJson = new StringBuilder("{\"seats\": {");
            var firstElement = true;
            foreach (var seat in this._seats)
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
    }
}