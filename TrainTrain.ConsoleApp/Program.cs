using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TrainTrain.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var train = args[0];
            var seats = int.Parse(args[1]);

            var manager = new TicketManager();

            var jsonResult = manager.Reserve(train, seats);

            Console.Write(jsonResult.Result);

            Console.WriteLine("Type <enter> to exit.");
            Console.ReadLine();
        }
    }

    public class TicketManager
    {
        public async Task<string> Reserve(string train, int seats)
        {
            var result = string.Empty;

            string JsonTrainTopology;
            
            // get the train topology
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

            result = JsonTrainTopology;

            return result;
        }
    }
}