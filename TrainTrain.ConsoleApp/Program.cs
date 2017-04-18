using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTrain.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var train = args[0];
            var seats = int.Parse(args[1]);

            var manager = new TicketManager();

            string jsonResult = manager.Reserve(train, seats);
        }
    }

    public class TicketManager
    {
        public string Reserve(string train, int seats)
        {
            //var train
            throw new NotImplementedException();
        }
    }
}
