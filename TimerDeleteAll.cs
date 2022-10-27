using System;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MySecondVSAppFunctionJD
{
    public class TimerDeleteAll
    {
        [FunctionName("TimerDeleteAll")]
        public static async Task Run([TimerTrigger("*/30 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            try
            {

                var str = Environment.GetEnvironmentVariable("sqldb_connection");
                using (SqlConnection conn = new SqlConnection(str))
                {
                    conn.Open();
                    var text = $"DELETE FROM [dbo].[Student]";

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

                log.LogError(e.Message);
            }

        }
    }
}
