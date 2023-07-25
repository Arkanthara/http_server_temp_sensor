using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using littlemichelserver.TemperatureSensorDB;

namespace littlemichelserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TempSensorController : ControllerBase
    {
        [HttpPost("{temp}")]
        public string Post(string temp)
        {
            string[] data = temp.Split(";");
            if (data.Length != 2)
            {
                //throw new ArgumentException("Data must be in the form: temperature;MAC");
                return "Data must be in the form: MAC;temperature";
            }
            SQLDB.InsertLine(data[0], data[1], DateTime.Now.ToString());
            return "That's ok !";
        }

        [HttpGet]
        public string Get()
        {
            return SQLDB.PrintTable();
        }
    }
}
