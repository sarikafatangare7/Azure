using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus; //Fo sending values to service queue

namespace queueSenderAzure
{
    class Program
    {
        /*
         * Connection sting for your Service Bus request
         */

        static string connectionstring = "Endpoint=sb://sarikaservicebus1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=un1eEih0ZbBruy/dPe6jiJyWSv2RQzKxSgl9zoLacDA=";
        static string queuename = "myqueuesarika";
        static ServiceBusClient client;
        static ServiceBusSender sender;
        private const int NumofMessages = 5;
        static async Task Main(string[] args)
        {
            Console.WriteLine("welcome to the Demo on Implementing Sevice Bus Queue!!! ");
            //Creating Client obj for sending the msg
            client = new ServiceBusClient(connectionstring);
            sender = client.CreateSender(queuename);

            //creating a Batch
            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();
            for(int i=1;i<NumofMessages;i++)
            {
                if(!messageBatch.TryAddMessage(new ServiceBusMessage($"Message {i}")))
                {
                    throw new Exception($" The Message {i} is roo large to fit in the batch");
                }
            }
            try
            {
                //use the poducer client to send the batch of message to the service Bus
                await sender.SendMessagesAsync(messageBatch);
                   Console.WriteLine ($"A Batch of {NumofMessages} messages has been published");
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception Caught" + e.Message);
                throw;
            }
            finally
            {
                //calling DisposeAsync on a client type is required to ensue that network resource and other
                //unmanaged objects are prperly cleaned up

                await sender.DisposeAsync();
                await client.DisposeAsync();

            }
            Console.WriteLine("Sending message to  queue !!!! ");

        }
    }
}
