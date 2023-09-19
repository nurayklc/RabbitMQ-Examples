using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://rxniewnv:q7meGNULORnRbaxpZqUDUhitAarKxkAC@woodpecker.rmq.cloudamqp.com/rxniewnv");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "headers-exchange", type: ExchangeType.Headers);

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes($"Hello {i}");
    Console.Write("Enter the header value: ");
    string value = Console.ReadLine();

    IBasicProperties properties = channel.CreateBasicProperties();
    properties.Headers = new Dictionary<string, object>
    {
        ["no"] = value
    };

    channel.BasicPublish(
        exchange: "headers-exchange",
        routingKey: String.Empty,
        body: message,
        basicProperties: properties
        );
}

Console.Read();