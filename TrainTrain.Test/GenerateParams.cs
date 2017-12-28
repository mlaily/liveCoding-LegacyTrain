namespace TrainTrain.Test
{
    public struct GenerateParams
    {
        public int CoachesCount { get; set; }
        public int SeatsCountPerCoach { get; set; }
        public string BookingReference { get; set; }
        public int SeatsAlreadyReservedCount { get; set; }
        public int CoachNumberWhereReserved { get; set; }
    }
}