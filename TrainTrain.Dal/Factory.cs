namespace TrainTrain.Dal
{
    public class Factory
    {
        public static IRepositoryEntity<TrainEntity> Create()
        {
            return new RepositoryEntity();
        }
    }
}