using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TrainTrain.ConsoleApp
{
    public class WebTicketManager
    {
        private const string uriBookingReferenceService = "http://localhost:9999";
        private const string urITrainDataService = "http://localhost:50680";

        public async Task<string> Reserve(string train, int seats)
        {
            var result = string.Empty;
            string bookingRef;

            // get the train topology
            var JsonTrainTopology = await GetTrain(train);

            result = JsonTrainTopology;

            var trainInst = new Train(JsonTrainTopology);

            if ((trainInst.MaxSeat - trainInst.ReservedSeats) > Math.Floor(ThreasholdManager.GetMaxRes() * trainInst.MaxSeat))
            {
                var numberOfReserv = 0;
                // find seats to reserve
                var availableSeats = trainInst.Seats.Where(s => s.BookingRef == string.Empty).Take(seats);

                int count = 0;
                foreach (var seat in availableSeats)
                    count++;

                if (count != seats)
                {
                    return $"{{\"train_id\": \"{train}\", \"booking_reference\": \"\", \"seats\": []}}";
                }
                else
                {

                    using (var client = new HttpClient())
                    {
                        bookingRef = await GetBookRef(client);
                    }

                    var reservedSets = 0;
                    foreach (var availableSeat in availableSeats)
                    {
                        availableSeat.BookingRef = bookingRef;
                        numberOfReserv++;
                    }
                }

                if (numberOfReserv == seats)
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(urITrainDataService);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        // HTTP POST
                        //var obj= new object();
                        //JsonConvert.SerializeObject(obj);
                        HttpContent resJSON = new StringContent(BuildPostContent(train, bookingRef, availableSeats), Encoding.UTF8, "application/json");

                        var response = await client.PostAsync($"reserve", resJSON);
                        response.EnsureSuccessStatusCode();


                        var JsonNewTrainTopology = await response.Content.ReadAsStringAsync();

                        //Check(JsonTrainTopology);

                        return $"{{\"train_id\": \"{train}\", \"booking_reference\": \"\", \"seats\": [TODOD]}}";
                    }
                }
            }

            return $"{{\"train_id\": \"{train}\", \"booking_reference\": \"\", \"seats\": []}}";
        }

        private static string BuildPostContent(string trainId, string bookingRef, IEnumerable<Seat> availableSeats)
        {
            var seats = new StringBuilder("[");
            bool firstTime = true;

            foreach (var availableSeat in availableSeats)
            {
                if (!firstTime)
                {
                    seats.Append(", ");
                }
                else
                {
                    firstTime = false;
                }

                seats.Append($"\"{availableSeat.SeatNumber}{availableSeat.CoachName}\"");
            }
            seats.Append("]");

            var result = $"{{\r\n\t\"train_id\": \"{trainId}\",\r\n\t\"seats\": {seats.ToString()},\r\n\t\"booking_reference\": \"{bookingRef}\"\r\n}}";

            return result;
        }

        protected async Task<string> GetTrain(string train)
        {
            string JsonTrainTopology;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urITrainDataService);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                var response = await client.GetAsync($"api/data_for_train/{train}");
                response.EnsureSuccessStatusCode();
                JsonTrainTopology = await response.Content.ReadAsStringAsync();
            }
            return JsonTrainTopology;
        }

        protected async Task<string> GetBookRef(HttpClient client)
        {
            return GetBookingReference();

            client.BaseAddress = new Uri(uriBookingReferenceService);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // HTTP GET
            var response = await client.GetAsync($"/booking_reference");
            response.EnsureSuccessStatusCode();

            var bookingRef = await response.Content.ReadAsStringAsync();
            return bookingRef;
        }

        private static readonly Random random = new Random();

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private string GetBookingReference()
        {
            return RandomString(6);
        }
    }
}