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
using System.Collections.Generic;
using MySecondVSAppFunctionJD.Modelo;
using System.Net.Http;
using System.Text;

namespace MySecondVSAppFunctionJD
{
    public static class GetAllStudents
    {
        [FunctionName("GetAllStudents")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetAllStudents.");
            string error = null;
            List<Student> students = new();

            try
            {

                var str = Environment.GetEnvironmentVariable("sqldb_connection");
                using (SqlConnection conn = new SqlConnection(str))
                {
                    conn.Open();
                    var text = "SELECT id, name, surname, address FROM [dbo].[Student]";

                    using (SqlCommand cmd = new SqlCommand(text, conn))
                    {
                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Student student = new Student();
                                student.id = reader.GetInt32(0);
                                student.name = reader.GetString(1);
                                student.surname = reader.GetString(2);
                                student.address = reader.GetString(3);

                                students.Add(student);
                            }
                        }
                        
                    }
                }
            }
            catch (Exception e)
            {

                error = e.Message;
            }

            return string.IsNullOrEmpty(error) ?
                new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(students), Encoding.UTF8, "application/json")
                }
                :new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(JsonConvert.SerializeObject("Error procesando la solicitud. " + error), Encoding.UTF8, "application/json")
                };
        }
    }
}
