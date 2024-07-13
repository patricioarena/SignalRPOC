using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace session_api.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public string[] Get()
        {
            string[] collection = new string[]  { "value1", "value2" };
            return collection;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public JObject Get(int id)
        {
            JObject JSON = new JObject();
            JSON.Add("GET", new JObject(new JProperty(new JProperty("id", id))));
            return JSON;
        }

        // POST api/values
        [HttpPost]
        public JObject Post([FromBody] string value)
        {
            JObject JSON = new JObject();
            JSON.Add("POST", new JObject(new JProperty(new JProperty("value", value))));
            return JSON;
        }

        // POST api/values/5
        [HttpPost("{id}")]
        public JObject Post(int id, [FromBody]JObject data)
        {
            JObject JSON = new JObject();
            JSON.Add("POST", new JObject(new JProperty("id", id.ToString()), new JProperty("data", data)));
            return JSON;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public JObject Put(int id, [FromBody] string value)
        {
            JObject JSON = new JObject();
            JSON.Add("PUT", new JObject(new JProperty("id", id.ToString()), new JProperty("value", value)));
            return JSON;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public JObject Delete(int id)
        {
            JObject JSON = new JObject();
            JSON.Add("DELETE", new JObject(new JProperty("id", id)));
            return JSON;
        }
    }
}
