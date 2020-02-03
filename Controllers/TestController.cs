using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspnet_core_api.Data;
using aspnet_core_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace aspnet_core_api.Controllers
{
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TestController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET api/values
        //[HttpGet]
        //public string[] Get()
        //{
        //    string[] collection = new string[]  { "value1", "value2" };
        //    return collection;
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public JObject Get(int id)
        //{
        //    JObject JSON = new JObject();
        //    JSON.Add("GET", new JObject(new JProperty(new JProperty("id", id))));
        //    return JSON;
        //}

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
            using (var db = new ApplicationDbContext())
            {
                DatosPersonales user = new DatosPersonales();
                user.PersonaID = (Guid)data.GetValue("personaID");
                user.Nombre = data.GetValue("nombre").ToString();
                user.Apellido = data.GetValue("apellido").ToString();
                user.FechaDeNac = (DateTime)data.GetValue("fechaDeNac");
                user.TEL = (int)data.GetValue("tel");
                user.CEL = (int)data.GetValue("cel");
                user.Email = data.GetValue("email").ToString();

                //user.Nombre = data.Nombre;
                //user.Apellido = data.Apellido;
                //user.FechaDeNac = (DateTime)data.FechaDeNac;
                //user.TEL = (int)data.TEL;
                //user.CEL = (int)data.CEL;
                //user.Email = data.Email;

                //user.Domicilio.Pais = data.Domicilio.Pais;
                //user.Domicilio.Provincia = data.Domicilio.Provincia;
                //user.Domicilio.Partido = data.Domicilio.Partido;
                //user.Domicilio.Localidad = data.Domicilio.Localidad;
                //user.Domicilio.Calle = data.Domicilio.Calle;
                //user.Domicilio.Numero = data.Domicilio.Numero;
                //user.Domicilio.Piso = data.Domicilio.Piso;
                //user.Domicilio.Depto = data.Domicilio.Depto;
                //user.Domicilio.CodPostal = data.Domicilio.CodPostal;

                //Domicilio domicilio = new Domicilio();

                db.DatosPersonales.Add(user);
                //db.Domicilio.Add(user.Domicilio);
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
        [HttpDelete("{id}")]
        public JObject Delete(int id)
        {
            JObject JSON = new JObject();
            JSON.Add("DELETE", new JObject(new JProperty("id", id)));
            return JSON;
        }
    }
}
