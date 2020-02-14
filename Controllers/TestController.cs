using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspnet_core_api.Data;
using aspnet_core_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace aspnet_core_api.Controllers
{
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ApplicationDbContext _Context;
        private readonly IConfiguration _Configuration;
        private readonly string _ConnectionString;

        public TestController(ApplicationDbContext context, IConfiguration configuration)
        {
            _Configuration = configuration;
            _Context = context;
            //_ConnectionString = configuration.GetConnectionString("SQLite");
            _ConnectionString = configuration.GetConnectionString("SQLServer");

        }

        // GET api/values
        [HttpGet]
        public List<DatosPersonales> Get()
        {
            List<DatosPersonales> listaPersonas = _Context.DatosPersonales.Include(dp => dp.Domicilio).ToList();
            return listaPersonas;
        }

        //// GET api/values/5
        [HttpGet("{personaID}")]
        public DatosPersonales Get(Guid personaID)
        {
            DatosPersonales persona = _Context.DatosPersonales.Where(e => e.PersonaID.Equals(personaID)).Include(dp => dp.Domicilio).FirstOrDefault();
            return persona;
        }

        // POST api/values
        //[HttpPost]
        //public JObject Post([FromBody] string value)
        //{
        //    JObject JSON = new JObject();
        //    JSON.Add("POST", new JObject(new JProperty(new JProperty("value", value))));
        //    return JSON;
        //}

        // POST api/values/5
        //[HttpPost("{id}")]
        //public async Task<JObject> PostAsync(int id, [FromBody]JObject data)
        //{
        //    JObject JSON = new JObject();
        //    JSON.Add("POST", new JObject(new JProperty("id", id.ToString()), new JProperty("data", data)));
        //    return JSON;
        //}


        // POST api/values/5
        [HttpPost]
        public async Task<JObject> PostAsync([FromBody]JObject data)
        {

            using (var db = new ApplicationDbContext(_ConnectionString))
            {
                DatosPersonales user = data.ToObject<DatosPersonales>();
                user.PersonaID = new Guid();
                user.Domicilio.DomicilioID = new Guid();

                //Domicilio domicilio = data.ToObject<DatosPersonales>().Domicilio;
                //user.domicilio.DomicilioID = new Guid();

                db.DatosPersonales.Add(user);
                db.Domicilio.Add(user.Domicilio);

                await db.SaveChangesAsync();
            }

            JObject JSON = new JObject();
            JSON.Add("POST", new JObject(new JProperty(new JProperty("value", data))));
            return JSON;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public JObject Put(int id, [FromBody] DatosPersonales value)
        {
            JObject JSON = new JObject();
            JSON.Add("PUT", new JObject(new JProperty("id", id.ToString()), new JProperty("value", value)));
            return JSON;
        }

        // DELETE api/values/5
        //[HttpDelete("{id}")]
        //public JObject Delete(int id)
        //{
        //    JObject JSON = new JObject();
        //    JSON.Add("DELETE", new JObject(new JProperty("id", id)));
        //    return JSON;
        //}
    }
}
