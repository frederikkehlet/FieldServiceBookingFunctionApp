using Azure.Messaging.ServiceBus;
using System.Configuration;
using System.Threading.Tasks;

namespace SendMessageToSBQPlugin.Logic
{
    public class SBClient : ISBClient
    {
        private readonly ServiceBusClient _client;
        private readonly ServiceBusSender _sender;

        public SBClient()
        {
            var clientOptions = new ServiceBusClientOptions()
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };

            string connectionString = ConfigurationManager.AppSettings["SbqConnectionString"];
            string queueName = ConfigurationManager.AppSettings["SbqQueueName"];

            _client = new ServiceBusClient(connectionString, clientOptions);
            _sender = _client.CreateSender(queueName);
        }

        public async Task SendMessageAsync(string message)
        {
            await _sender.SendMessageAsync(new ServiceBusMessage(message));
        }
    }
}
