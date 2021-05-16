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
            _ConnectionString = configuration.GetConnectionString("SQLite");
            //_ConnectionString = configuration.GetConnectionString("SQLServer");

        }

        // GET api/values
        [HttpGet]
        public List<DatosPersonales> Get()
        {
            List<DatosPersonales> listaPersonas = _Context.DatosPersonales
                    .Include(dp => dp.Domicilios)
                    .Include(es => es.Estudios)
                    .Include(idio => idio.Idiomas)
                    .Include(exp => exp.Experiencias)
                    .Include(conT => conT.ConocimientosTecnicos)
                    .Include(conA => conA.ConocimientosTecnicos)
                    .ToList();

            return listaPersonas;
        }
        //Esto es lo mas de zamora !!!! 

        //// GET api/values/5
        [HttpGet("{personaID}")]
        public DatosPersonales Get(Guid personaID)
        {
            DatosPersonales persona = _Context.DatosPersonales
                .Where(e => e.PersonaID.Equals(personaID))
                    .Include(dp => dp.Domicilios)
                    .Include(es => es.Estudios)
                    .Include(idio => idio.Idiomas)
                    .Include(exp => exp.Experiencias)
                    .Include(conT => conT.ConocimientosTecnicos)
                    .Include(conA => conA.ConocimientosTecnicos)
                        .FirstOrDefault();
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
        /// <summary>
        /// </summary>
        /// <remarks>
        /// 
        /// Object Example
        /// 
        ///     {
        ///     "nombre": "Patricio Ernesto Antonio",
        ///     "apellido": "Arena",
        ///     "fechaDeNac": "1987-07-09",
        ///     "tel": "42740810",
        ///     "cel": "1133142754",
        ///     "email": "patricio.e.arena@gmail.com",
        ///     "domicilios": [
        ///       {
        ///           "pais": "Argentina",
        ///           "provincia": "Buenos Aires",
        ///           "localidad": "Florencio Varela",
        ///           "partido": "Florencio Varela",
        ///           "codPostal": 1888,
        ///           "calle": "Madrid",
        ///           "numero": 55,
        ///           "piso": 0,
        ///           "depto": "-"
        ///       }
        ///     ],
        ///     "estudios": [
        ///       {
        ///           "establecimiento": "Universidad Nacional Arturo Jauretche",
        ///           "titulo": "string",
        ///           "disciplina": "Ingenieria Informatica",
        ///           "fechaDeInicio": "2013-04-13",
        ///           "fechaDeFin": "2032-12-31",
        ///           "actExtra": "",
        ///           "descripcion": ""
        ///       }
        ///     ],
        ///     "idiomas": [
        ///       {
        ///         "idioma": "Ingles",
        ///         "nivelEscrito": 1,
        ///         "nivelLectura": 1,
        ///         "nivelOral": 1
        ///       }
        ///     ],
        ///     "experiencias": [
        ///       {
        ///         "fechaDeInicio": "2020-02-17",
        ///         "fechaDeFin": "2020-02-17",
        ///         "cargo": "string",
        ///         "tipoDeEmpleo": "string",
        ///         "empresa": "string",
        ///         "ubicacion": "string",
        ///         "descripcion": "string"
        ///       }
        ///     ],
        ///     "conocimientosTecnicos": [
        ///       {
        ///         "titulo": 1,
        ///         "conocimiento": "string",
        ///         "nivel": 1
        ///       }
        ///     ],
        ///     "conocimientosAdicionales": [
        ///       {
        ///         "titulo": "string",
        ///         "descripcion": "string"
        ///       }
        ///     ]
        ///     }
        ///     
        /// </remarks>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JObject> PostAsync([FromBody]JObject data)
        {

            using (var db = new ApplicationDbContext(_ConnectionString))
            {
                DatosPersonales user = data.ToObject<DatosPersonales>();
                db.DatosPersonales.Add(user);
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
