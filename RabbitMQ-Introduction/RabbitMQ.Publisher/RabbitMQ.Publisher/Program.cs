using RabbitMQ.Client;
using System.Text;

// Create Connection
ConnectionFactory connectionFactory = new ConnectionFactory();
connectionFactory.Uri = new Uri("amqps://rxniewnv:q7meGNULORnRbaxpZqUDUhitAarKxkAC@woodpecker.rmq.cloudamqp.com/rxniewnv");

// Create and Connect Channel
using IConnection connection = connectionFactory.CreateConnection();
using IModel channel = connection.CreateModel();

// Create Queue
channel.QueueDeclare(queue: "example-queue", exclusive: false);

// Sent to the Queue
for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes("merhabaaaaaaaaa" + i);
    channel.BasicPublish(exchange: "", "example-queue", body: message);
}

Console.ReadKey();