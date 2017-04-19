namespace AcmeCorp.TrainDataService.Models
{
    public interface IProvideTrain
    {
        Train GetTrain(string trainId);
    }
}