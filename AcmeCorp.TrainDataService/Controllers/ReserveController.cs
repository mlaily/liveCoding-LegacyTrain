using AcmeCorp.TrainDataService.Models;
using Microsoft.AspNetCore.Mvc;

namespace AcmeCorp.TrainDataService.Controllers
{
    [Route("api/reserve")]
    public class ReserveController : Controller
    {
        // PUT on 
        // http://localhost:50680/api/reserve
        // with JSON payload:
        //  {
        //      "train_id": "5FSdR",
        //      "seats": ["4A", "5A"],
        //      "booking_reference": "Td98kms"
        //  }
        //
        private readonly IProvideTrain trainProvider;

        public ReserveController(IProvideTrain trainProvider)
        {
            this.trainProvider = trainProvider;
        }

        // POST api/data_for_train
        [HttpPost]
        public void Post([FromBody] TrainUpdateDTO value)
        {
            trainProvider.UpdateTrainReservations(value);
        }

        //// PUT api/data_for_train/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/data_for_train/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}