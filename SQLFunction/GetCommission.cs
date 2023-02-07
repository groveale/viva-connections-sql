using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace groverale
{
    public static class GetCommission
    {
        [FunctionName("GetCommission")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            // Get the userEmail query parameters from the request
            string userEmail = req.Query["userEmail"];

            // Read the request body and deserialize it into a dynamic object
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            
            // Set the userEmail variable using the query parameters or the values from the request body
            userEmail = userEmail ?? data?.userEmail;

            try
            {
                // Load the settings for the SQLHelper
                var settings = Settings.LoadSettings();
                
                // Get the commission for the user, daily and weekly
                var dailyCommission = SQLHelper.GetEmployeeCommissionDaily(userEmail, settings);
                var weeklyCommission = SQLHelper.GetEmployeeCommissionWeekly(userEmail, settings);

                
                // Return the result as an OkObjectResult with the SQLCommissionResponse object
                // Divide values by 10 to get pounds
                return new OkObjectResult(new SQLCommissionResponse { Daily = dailyCommission / 100, Weekly = weeklyCommission / 100});
            }
            catch (Exception ex)
            {
                // If there is an error, return a BadRequestObjectResult with the error message
                return new BadRequestObjectResult($"Error in request: {ex.Message}");
            }
        }
    }

    // Define a class to store the response of the Azure Function
    public class SQLCommissionResponse
    {
        // Properties to store the Daily and Weekly commission
        public double Daily {get;set;}
        public double Weekly {get;set;}
    }
}
