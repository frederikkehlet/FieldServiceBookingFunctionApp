using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System;

namespace FieldServiceReservationToOutlookEventFunction
{
    public class FieldServiceBookingFunction
    {
        [FunctionName("sbq-field-service-booking-function")]
        public async Task Run([ServiceBusTrigger("field-service-reservation-queue",
            Connection = "SB_CONNECTION_STRING")]string myQueueItem, ILogger log)
        {
            try
            {

               
            }
            catch (Exception ex)
            {
                log.LogError($"An error occured: {ex.Message}.\nStack trace: {ex.StackTrace}");
            }
        }
    }
}
