using System;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Handlers;
using Domain.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FieldServiceBookingFunction
{
    public class FieldServiceBookingFunction
    {
        private readonly IBookableResourceBookingToOutlookEventHandler _bookingToOutlookEventHandler;

        public FieldServiceBookingFunction(
            IBookableResourceBookingToOutlookEventHandler bookingToOutlookEventHandler)
        {
            _bookingToOutlookEventHandler = bookingToOutlookEventHandler;
        }

        [FunctionName("sbq-field-service-booking-function")]
        public async Task Run([ServiceBusTrigger("field-service-reservation-queue", 
            Connection = "SB_CONNECTION_STRING")]string myQueueItem, ILogger log)
        {          
            try
            {
                PluginStepMessage message;
                message = JsonSerializer.Deserialize<PluginStepMessage>(myQueueItem);
                log.LogInformation(JsonSerializer.Serialize(message));

                switch (message.MessageName)
                {
                    case "Create":
                        await _bookingToOutlookEventHandler.HandleCreate(message);
                        break;
                    case "Update":
                        await _bookingToOutlookEventHandler.HandleUpdate(message);
                        break;
                    case "Delete":
                        await _bookingToOutlookEventHandler.HandleDelete(message);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                log.LogError($"An error occured: {ex.Message}.\nStack trace: {ex.StackTrace}");
            }
        }
    }
}
