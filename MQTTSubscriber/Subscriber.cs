using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Text;
using System.Threading.Tasks;

namespace MQTTSubscriber // Note: actual namespace depends on the project name.
{
    public class Subscriber
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
            client.UseConnectedHandler(async e =>
            {
                Console.WriteLine("Conectado con exito al broker");
                var topicFilter = new MqttTopicFilterBuilder()
                                        .WithTopic("Andres")
                                        .Build();
                await client.SubscribeAsync(topicFilter);
            });

            client.UseDisconnectedHandler(e =>
            {
                Console.WriteLine("Desconectado del broker con exito");
            });

            client.UseApplicationMessageReceivedHandler(e =>
            {
                Console.WriteLine($"Mensaje recibido - {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
            });

            await client.ConnectAsync(options);

            Console.ReadLine();

            await client.DisconnectAsync();
        }
    }
}

