using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://rxniewnv:q7meGNULORnRbaxpZqUDUhitAarKxkAC@woodpecker.rmq.cloudamqp.com/rxniewnv");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();


// Step 1:
channel.ExchangeDeclare(exchange: "direct-exchange", type: ExchangeType.Direct);

// Step 2: 
string queueName = channel.QueueDeclare().QueueName;

// Step 3: 
channel.QueueBind(
    queue: queueName,
    exchange: "direct-exchange",
    routingKey: "direct-queue");

EventingBasicConsumer consumer = new(channel);

channel.BasicConsume(
    queue: queueName,
    autoAck: true,
    consumer: consumer);

consumer.Received += (sender, e) =>
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
};

Console.Read();