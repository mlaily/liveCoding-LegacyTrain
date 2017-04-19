namespace AcmeCorp.TrainDataService.Models
{
    public class Train
    {
        private string trainId;

        public Train(string trainId)
        {
            this.trainId = trainId;
        }

        public override string ToString()
        {
            return
                "{\"seats\": {\"1A\": {\"booking_reference\": \"\", \"seat_number\": \"1\", \"coach\": \"A\"}, \"2A\": {\"booking_reference\": \"\", \"seat_number\": \"2\", \"coach\": \"A\"}}}";
        }
    }
}