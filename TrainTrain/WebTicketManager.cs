using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TrainTrain.Dal;
using TrainTrain.Dal.Entities;

namespace TrainTrain
{
    public class WebTicketManager
    {
        private const string uriBookingReferenceService = "http://localhost:51691/";
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

                var reservedSets = 0;


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

                    foreach (var availableSeat in availableSeats)
                    {
                        availableSeat.BookingRef = bookingRef;
                        numberOfReserv++;
                        reservedSets++;
                    }
                }

                if (numberOfReserv == seats)
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(urITrainDataService);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        if (reservedSets == 0)
                        {
                            Console.WriteLine("Reserved seat(s): ", reservedSets);
                        }

                        // HTTP POST
                        HttpContent resJSON = new StringContent(BuildPostContent(train, bookingRef, availableSeats), Encoding.UTF8, "application/json");
                        var response = await client.PostAsync($"reserve", resJSON);

                        response.EnsureSuccessStatusCode();

                        var todod = "[TODOD]";

                        await Task.Run(()=> Save(trainInst, train, bookingRef));

                        return $"{{\"train_id\": \"{train}\", \"booking_reference\": \"{bookingRef}\", \"seats\": {DumpSeats(availableSeats)}}}";
                    }
                }
            }

            return $"{{\"train_id\": \"{train}\", \"booking_reference\": \"\", \"seats\": []}}";
        }

        private string DumpSeats(IEnumerable<Seat> seats)
        {
            var sb = new StringBuilder("[");

            var firstTime = true;
            foreach (var seat in seats)
            {
                if (!firstTime)
                {
                    sb.Append(", ");
                }
                else
                {
                    firstTime = false;
                }

                sb.Append($"\"{seat.SeatNumber}{seat.CoachName}\"");
            }

            sb.Append("]");

            return sb.ToString();
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
            client.BaseAddress = new Uri(uriBookingReferenceService);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // HTTP GET
            var response = await client.GetAsync($"/booking_reference");
            response.EnsureSuccessStatusCode();

            var bookingRef = await response.Content.ReadAsStringAsync();
            return bookingRef;
        }

        private static void Save(Train trainInst, string trainId, string bookingRef)
        {
            var trainEntity = new TrainEntity { TrainId = trainId };
            foreach (var seat in trainInst.Seats)
            {
                trainEntity.Seats.Add(new SeatEntity { TrainId = trainId, BookingRef = bookingRef, CoachName = seat.CoachName, SeatNumber = seat.SeatNumber });
            }
            Factory.Create().Save(trainEntity);
        }

    }
}