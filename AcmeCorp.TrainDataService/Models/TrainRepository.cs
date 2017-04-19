namespace AcmeCorp.TrainDataService.Models
{
    public class TrainRepository : IProvideTrain
    {
        public Train GetTrain(string trainId)
        {
            return new Train(trainId);
        }
    }
}