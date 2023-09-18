using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://rxniewnv:q7meGNULORnRbaxpZqUDUhitAarKxkAC@woodpecker.rmq.cloudamqp.com/rxniewnv");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "direct-exchange", type: ExchangeType.Direct);

while (true)
{
    Console.Write("Message: ");
    string message = Console.ReadLine();
    byte[] byteMessage = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(
        exchange: "direct-exchange",
        routingKey: "direct-queue",
        body: byteMessage);
}

Console.Read();  