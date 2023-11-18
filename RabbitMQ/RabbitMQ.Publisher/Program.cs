using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace RabbitMQ.Publisher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://wylxhthv:9gEUm_Hi-PGavdDsheGhvL106sliPAi5@shark.rmq.cloudamqp.com/wylxhthv ");

            using var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            //birinci parametre kuyruğumuzun ismi
            //ikinci parametre kuyruğumuzun geçici ya da kalıcı olması (true = kalıcı, false = geçici)
            //üçüncü parametre kuyruğumuzun farklı processler tarafından erişilip erişilemeyeceğini belirtir. (true = sadece bu channel üzerinden erişilir, false = farklı processler tarafından da erişilebilir)
            //dördüncü parametre kuyruğumuzun otomatik silinmemesini sağlar

            channel.QueueDeclare("hello-queue", true, false, false);
            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                string message = $"Message: {x}";
                var messageBody = Encoding.UTF8.GetBytes(message);

                //arada exchange olmadığından dolayı (default exchange) string.Empty kullandık.
                channel.BasicPublish(string.Empty,"hello-queue", null, messageBody);
                
                Console.WriteLine($"Mesaj başarılı bir şekilde gönderilmiştir: {message}");
            }
            );

            //string message = "Hello World!";

            //var messageBoddy = Encoding.UTF8.GetBytes(message);

            
            //channel.BasicPublish(string.Empty, "hello-queue", null, messageBoddy);

            //Console.WriteLine("Mesaj gönderilmiştir.");
            Console.ReadLine();


        }
    }
}
