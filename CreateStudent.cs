using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace MySecondVSAppFunctionJD
{
    public static class CreateStudent
    {
        [FunctionName("CreateStudent")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string error = null;
            string name = req.Query["name"];
            string surname = req.Query["surname"];
            string address = req.Query["address"];

            try
            {

                var str = Environment.GetEnvironmentVariable("sqldb_connection");
                using (SqlConnection conn = new SqlConnection(str))
                {
                    conn.Open();
                    var text = $"INSERT INTO [dbo].[Student] ([Name],[Surname],[Address]) VALUES('{name}' ,'{surname}' , '{address}')";

                    using (SqlCommand cmd = new SqlCommand(text, conn))
                    {
                        // Execute the command and log the # rows affected.
                        var rows = await cmd.ExecuteNonQueryAsync();
                        log.LogInformation($"{rows} rows were created");
                    }
                }
            }
            catch (Exception e)
            {

                error = e.Message;
            }




            string responseMessage = string.IsNullOrEmpty(error) ? $"Estudiante {name} {surname}, insertado correctamente" : "Se ha producido un error: error";

            return new OkObjectResult(responseMessage);
        }
    }
}
