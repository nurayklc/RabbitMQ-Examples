using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://rxniewnv:q7meGNULORnRbaxpZqUDUhitAarKxkAC@woodpecker.rmq.cloudamqp.com/rxniewnv");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "fanout-exchange", type: ExchangeType.Fanout);

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes($"Message {i}");
    channel.BasicPublish(
        exchange: "fanout-exchange", 
        routingKey: String.Empty,
        body: message);
}

Console.Read();