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
    public static class DeleteStudent
    {
        [FunctionName("DeleteStudent")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("DeleteStudent.");
            string error = null;

            int id = Int32.Parse(req.Query["id"]);

            try
            {

                var str = Environment.GetEnvironmentVariable("sqldb_connection");
                using (SqlConnection conn = new SqlConnection(str))
                {
                    conn.Open();
                    var text = $"DELETE FROM [dbo].[Student] WHERE id = {id}";

                    using (SqlCommand cmd = new SqlCommand(text, conn))
                    {
                        // Execute the command and log the # rows affected.
                        var rows = await cmd.ExecuteNonQueryAsync();
                        log.LogInformation($"{rows} rows were deleted");
                    }
                }
            }
            catch (Exception e)
            {

                error = e.Message;
            }




            string responseMessage = string.IsNullOrEmpty(error) ? $"Estudiante {id}, eliminado correctamente" : $"Se ha producido un error: {error}";

            return new OkObjectResult(responseMessage);
        }
    }
}
