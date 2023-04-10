using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FieldServiceBookingFunctionApp
{
    public class FieldServiceBookingFunction
    {
        [FunctionName("FieldServiceBookingFunction")]
        public void Run([QueueTrigger("field-service-reservation-queue", Connection = "SB_CONNECTION_STRING")]string myQueueItem, 
            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
