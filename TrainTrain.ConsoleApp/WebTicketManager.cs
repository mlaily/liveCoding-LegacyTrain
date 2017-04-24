using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TrainTrain.ConsoleApp
{
    public class WebTicketManager
    {
        public async Task<string> Reserve(string train, int seats)
        {
            var result = string.Empty;

            // get the train topology
            var JsonTrainTopology = await GetTrain(train);

            result = JsonTrainTopology;

            var trainInst = new Train(JsonTrainTopology);

            if ((trainInst.MaxSeat - trainInst.ReservedSeats) < Math.Floor(ThreasholdManager.GetMaxRes() * trainInst.MaxSeat))
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
                    string bookingRef;

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
                        client.BaseAddress = new Uri("http://localhost:6666");
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        // HTTP POST
                        //var obj= new object();
                        //JsonConvert.SerializeObject(obj);
                        HttpContent resJSON = new StringContent($"{{TODO}}", Encoding.UTF8, "application/json");

                        var response = await client.PostAsync($"/reserve", resJSON);
                        response.EnsureSuccessStatusCode();
                        var JsonNewTrainTopology = await response.Content.ReadAsStringAsync();

                        //Check(JsonTrainTopology);

                        return $"{{\"train_id\": \"{train}\", \"booking_reference\": \"\", \"seats\": [TODOD]}}";
                    }
                }
            }

            return $"{{\"train_id\": \"{train}\", \"booking_reference\": \"\", \"seats\": []}}";
        }

        protected async Task<string> GetTrain(string train)
        {
            string JsonTrainTopology;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:50680");
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
            client.BaseAddress = new Uri("http://localhost:9999");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // HTTP GET
            var response = await client.GetAsync($"/booking_reference");
            response.EnsureSuccessStatusCode();

            var bookingRef = await response.Content.ReadAsStringAsync();
            return bookingRef;
        }
    }
}