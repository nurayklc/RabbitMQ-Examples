
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://rxniewnv:q7meGNULORnRbaxpZqUDUhitAarKxkAC@woodpecker.rmq.cloudamqp.com/rxniewnv");

using IConnection connection = factory.CreateConnection();
using IModel channel  = connection.CreateModel();

channel.ExchangeDeclare(exchange: "fanout-exchange", type: ExchangeType.Fanout);


Console.Write("Enter the queue name: ");
string queueName = Console.ReadLine();

channel.QueueDeclare(
    queue: queueName,
    exclusive: false);

channel.QueueBind(
    queue: queueName,
    exchange: "fanout-exchange",
    routingKey: String.Empty);

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: queueName,
    consumer: consumer,
    autoAck: true
    );

consumer.Received += (sender, e) =>
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));    
};


Console.Read();