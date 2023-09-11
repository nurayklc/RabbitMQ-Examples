using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

// Create Connection
ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://rxniewnv:q7meGNULORnRbaxpZqUDUhitAarKxkAC@woodpecker.rmq.cloudamqp.com/rxniewnv");

// Create and Connect Channel
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

// Create Queue
channel.QueueDeclare(queue: "example-queue", exclusive: false);

// Read the Message to the Queue
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue:"example-queue", false, consumer: consumer);
consumer.Received += (sender, e) =>
{
    // Queue'ya gelen mesajın işlendiği kısımdır.
    // e.Body : Queue'daki mesajdır.
    // e.Body.Span veya e.Body.ToArray() : Queue'daki mesajı byte tipinde getirmektedir.
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
};

Console.ReadKey();