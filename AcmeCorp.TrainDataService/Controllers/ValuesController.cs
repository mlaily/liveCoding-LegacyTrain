using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace AcmeCorp.TrainDataService.Controllers
{
    [Route("api/data_for_train")]
    //[Route("api/[controller]")]
    public class TrainController : Controller
    {
        // GET api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/values/5
        [HttpGet("{id}")]
        //[HttpGet]
        public string Get(string id)
        {
            return $"train id: {id}";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
