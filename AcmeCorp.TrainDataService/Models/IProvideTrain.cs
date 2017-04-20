namespace AcmeCorp.TrainDataService.Models
{
    public interface IProvideTrain
    {
        Train GetTrain(string trainId);
        void UpdateTrainReservations(string jsonFormatForTrainUpdate);
    }
}