using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using userapi.Interfaces;

namespace userapi.Service {
    public class MessageProducer : IMessageProducer {
        public void SendingMessage<T>(T message) {
            var factory = new ConnectionFactory() {
                HostName = "localhost",
                UserName = "user",
                Password = "mypass",
                VirtualHost = "/"
            };

            var conn = factory.CreateConnection();
            using var channel = conn.CreateModel();

            channel.QueueDeclare("userAPI", durable: true, exclusive: false, autoDelete: false, arguments: null);
            var jsonString = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonString);

            channel.BasicPublish("", "userAPI", body: body);

        }
    }
}