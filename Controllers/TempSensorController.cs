using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using littlemichelserver.TemperatureSensorDB;
using System.Diagnostics;

namespace littlemichelserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TempSensorController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return SQLDB.PrintTable(id: true);
        }

        [HttpPost]
        public async Task<string> Post()
        {
            // We create a stream reader for read request body
            StreamReader reader = new StreamReader(Request.Body);

            // We retrieve the stream as a string
            string parameter = await reader.ReadToEndAsync();

            // Verification that we don't receive no data
            if (string.IsNullOrEmpty(parameter))
            {
                return "No data received";
            }
            
            // If we receive data, we try to insert then in the table
            int error = SQLDB.InsertLine(parameter);

            // If an error occurs, we send message information to the client
            if (error != 0)
            {
                return "Error when trying add line in the table. \nUsage:\nYou must send data on form MAC;temperature with no space between 'MAC' and ';', and ';' and 'temperature'";
            }

            // If all's right, we tell the client that all's right
            return "That's ok !";
        }

        [HttpDelete]
        public async Task<string> Delete()
        {
            // We initialize an instruction message
            string instructions = "Usage\ntrue:		clean all the table\nid:		delete line with id given\nAttention:	you must give just one parameter !!!";

            // We create a stream reader for read request body
            StreamReader reader = new StreamReader(Request.Body);

            // We retrieve the stream as a string
            string parameters = await reader.ReadToEndAsync();

            // We verify that we don't have no parameters given, and we send instructions to the client if necessary
            if (string.IsNullOrEmpty (parameters))
            {
                return instructions;
            }

            // We init two variable
            bool clean;
            int error = -1;

            // We try to convert parameters to bool
            try
            {
                clean = bool.Parse(parameters);
            }

            // If it don't work, we estimate that the parameter is an id, and we call our function with id parameter
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                error = SQLDB.DeleteLine(id: parameters);
                if (error != 0)
                {
                    return "Error when trying to delete line. Verify your id parameter\n" + instructions;
                }
                return "Delete: Success";
            }

            // Else, we execute our function with bool parameter
            error = SQLDB.DeleteLine(cleanTable: clean);
            if (error != 0)
            {
                return "Error when trying to clean table.Verify your boolean parameter\n" + instructions;
            }
            return "Delete: Success";
        }


        /* Use patch method for print table with options (Get method don't allow to give parameters...)
        [HttpPatch]
        public async Task<string> Patch()
        {
            string warning = "Warning: here the patch method is just used for print the table with parameters given. If true is given, the tab will be printed with id\n";
            StreamReader reader = new StreamReader(Request.Body);
            string parameters = await reader.ReadToEndAsync();
            if (string.IsNullOrEmpty (parameters))
            {
                return warning + SQLDB.PrintTable();
            }
            bool id;
            try
            {
                id = bool.Parse(parameters);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return warning + ex.Message;
            }
            return warning + SQLDB.PrintTable(id);
        }
*/
    }
}
