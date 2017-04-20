using Microsoft.AspNetCore.Mvc;
using AcmeCorp.TrainDataService.Models;

namespace AcmeCorp.TrainDataService.Controllers
{
    [Route("api/data_for_train")]
    //[Route("api/[controller]")]
    public class TrainController : Controller
    {
        private readonly IProvideTrain trainProvider;

        public TrainController(IProvideTrain trainProvider)
        {
            this.trainProvider = trainProvider;
        }

        // GET api/data_for_train
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/data_for_train/5FSdR
        [HttpGet("{trainId}")]
        //[HttpGet]
        public string Get(string trainId)
        {
            return this.trainProvider.GetTrain(trainId).ToString();
        }

        // POST api/data_for_train
        [HttpPost]
        public void Post([FromBody]string value)
        {
            this.trainProvider.UpdateTrainReservations(value);
        }

        // PUT api/data_for_train/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/data_for_train/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
