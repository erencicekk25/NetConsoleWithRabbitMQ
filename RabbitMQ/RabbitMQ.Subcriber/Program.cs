using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace RabbitMQ.Subcriber
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
            //dördüncü parametre kuyruğumuzun otomatik silinmemesini sağlar (false)
            //aynı kuyruğu subcribe tarafında oluştururken parametrelerin aynı olmasına dikkat etemiz gerek.
            //   channel.QueueDeclare("hello-queue", true, false, false);

            
            
            //channel.BasicQos(0, 6, false); her bir subcriber'e tek seferde 6şar mesaj gönderir
            //channel.BasicQos(0, 6, true);  sayıyı böler ve subcriberlara dağıtır.
            channel.BasicQos(0, 1, false);  

            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume("hello-queue", false, consumer);
            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                Thread.Sleep(1500);
                Console.WriteLine("Gelen mesaj: " + message);


                //sadece ilgili mesajın durumunu rapidMQ'ya bildir. Eğer true yapsaydık işlenmiş fakat geri bildirilmemiş tüm mesajları da bildirirdi.
                channel.BasicAck(e.DeliveryTag, false);
            };
            Console.ReadLine();
        }


    }
}
