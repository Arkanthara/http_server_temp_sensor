using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Runtime.CompilerServices;

namespace littlemichelserver.Controllers
{
    [Route("[controller]")]
   // [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public Dictionary<string, string> Get()
        {
            return Data.data_received;
        }
        [HttpPost("{last_name}/{first_name}")]
        public string Post(string last_name, string first_name)
        {
            bool error = Data.data_received.TryAdd(last_name, first_name);
            if (!error)
            {
                return "Person with name " + last_name + " already exist, and her last_name is: " + Data.data_received[last_name];
            }
            return "That's ok";

        }

        [HttpPut("{last_name}/{first_name}")]
        public string Put (string last_name, string first_name)
        {
            bool error = Data.data_received.TryAdd(last_name, first_name);
            if (!error)
            {
                Data.data_received[last_name] = first_name;
            }
            return "That's ok";
        }

        [HttpPatch("{last_name}/{first_name_to_modify}")]
        public string Patch(string last_name, string first_name_to_modify)
        {
            foreach(var item in Data.data_received.Keys)
            {
                if (item == last_name)
                {
                    Data.data_received[last_name] = first_name_to_modify;
                    return "That's ok";
                }
            }
            return "Person " + last_name + " not created";
        }

        [HttpDelete("{last_name}")]

        public string Delete(string last_name)
        {
            bool error = Data.data_received.Remove(last_name);
            if (!error)
            {
                return "Error: person " + last_name + " not found";
            }
            return "Person remove successfully";
        }


        [HttpHead]
        public string Head()
        {
            return Request.Headers.ToString();
        }

        [HttpOptions] public string Options() { return "I don't know what I can put here..."; }

    }
}
