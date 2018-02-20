using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TrainTrain.Domain;

namespace TrainTrain.Infra
{
    public class TrainDataService : ITrainDataService
    {
        private readonly string _urlTrainDataService;

        public TrainDataService(string urlTrainDataService)
        {
            _urlTrainDataService = urlTrainDataService;
        }

        public async Task<Train> GetTrain(string train)
        {
            using (var client = new HttpClient())
            {
                var value = new MediaTypeWithQualityHeaderValue("application/json");
                client.BaseAddress = new Uri(_urlTrainDataService);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(value);

                // HTTP GET
                var response = await client.GetAsync($"api/data_for_train/{train}");
                response.EnsureSuccessStatusCode();
                return  new Train(AdaptTrainTopology(await response.Content.ReadAsStringAsync()));
            }
        }

        public async Task Reserve(ReservationAttempt reservationAttempt)
        {
            using (var client = new HttpClient())
            {
                var value = new MediaTypeWithQualityHeaderValue("application/json");
                client.BaseAddress = new Uri(_urlTrainDataService);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(value);

                // HTTP POST
                HttpContent resJSON = new StringContent(BuildPostContent(reservationAttempt.TrainId, reservationAttempt.BookingReference, reservationAttempt.Seats),
                    Encoding.UTF8, "application/json");
                var response = await client.PostAsync("reserve", resJSON);

                response.EnsureSuccessStatusCode();
            }
        }

        private static string BuildPostContent(string trainId, string bookingRef, IEnumerable<Seat> availableSeats)
        {
            var seats = new StringBuilder("[");
            bool firstTime = true;

            foreach (var s in availableSeats)
            {
                if (!firstTime)
                {
                    seats.Append(", ");
                }
                else
                {
                    firstTime = false;
                }

                seats.Append(String.Format("\"{0}{1}\"", s.SeatNumber, s.CoachName));
            }

            seats.Append("]");

            var result = String.Format(
                "{{\r\n\t\"train_id\": \"{0}\",\r\n\t\"seats\": {1},\r\n\t\"booking_reference\": \"{2}\"\r\n}}",
                trainId, seats, bookingRef);

            return result;
        }

        public static List<Seat> AdaptTrainTopology(string trainTopology)
        {

            var seats = new List<Seat>();
            //var sample =
            //"{\"seats\": {\"1A\": {\"booking_reference\": \"\", \"seat_number\": \"1\", \"coach\": \"A\"}, \"2A\": {\"booking_reference\": \"\", \"seat_number\": \"2\", \"coach\": \"A\"}}}";

            // Forced to workaround with dynamic parsing since the received JSON is invalid format ;-(
            dynamic parsed = JsonConvert.DeserializeObject(trainTopology);

            foreach (var token in ((JContainer) parsed))
            {
                var allStuffs = ((JObject) ((JContainer) token).First);

                foreach (var stuff in allStuffs)
                {
                    var seat = stuff.Value.ToObject<SeatJsonPoco>();
                    seats.Add(new Seat(seat.coach, Int32.Parse(seat.seat_number), seat.booking_reference));

                }
            }

            return seats;
        }
    }
}