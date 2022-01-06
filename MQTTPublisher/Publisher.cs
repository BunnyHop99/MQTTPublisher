using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Threading.Tasks;

namespace MQTTPublisher
{
    public class Publisher
    {
        static async Task Main(string[] args)
        {
            var mqttFactory = new MqttFactory();
            var client = mqttFactory.CreateMqttClient();
            var options = new MqttClientOptionsBuilder()
                                .WithClientId(Guid.NewGuid().ToString())
                                .WithTcpServer("test.mosquitto.org", 1883)
                                .WithCleanSession()
                                .Build();
            client.UseConnectedHandler(e =>
            {
                Console.WriteLine("Conectado con exito al broker");
            });

            client.UseDisconnectedHandler(e =>
            {
                Console.WriteLine("Desconectado del broker con exito");
            });

            await client.ConnectAsync(options);

            Console.WriteLine("Presiona cualquier tecla para publicar");

            Console.ReadLine();

            await PublishMessageAsync(client);

            await client.DisconnectAsync();
        }

        private static async Task PublishMessageAsync(IMqttClient client)
        {
            string messagePayLoad = "Hola";
            var message = new MqttApplicationMessageBuilder()
                                .WithTopic("Andres")
                                .WithPayload(messagePayLoad)
                                .WithAtLeastOnceQoS()
                                .Build();
            if(client.IsConnected)
            {
                await client.PublishAsync(message);
            }
        }
    }
  
}