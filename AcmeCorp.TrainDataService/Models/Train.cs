using System.Collections.Generic;
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
            _seats.Add(new Seat("A", "43", ""));
            _seats.Add(new Seat("A", "44", "45klksnFaZ"));
        }

        public IEnumerable<Seat> seats => _seats;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);

            return
                "{\"seats\": {\"1A\": {\"booking_reference\": \"\", \"seat_number\": \"1\", \"coach\": \"A\"}, \"2A\": {\"booking_reference\": \"\", \"seat_number\": \"2\", \"coach\": \"A\"}}}";
        }
    }
}