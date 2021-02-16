using System;
using System.Threading.Tasks;
using zhigalin.test.Connectors;
using zhigalin.test.Models;

namespace zhigalin.test
{
    public class Program
    {
        
        static async Task Main(string[] args)
        {
            var connector = new ExternalSystemConnector();
            var queue = new DocumentQueue(connector);

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < new Random().Next(1, 20); j++)
                {
                    queue.Enqueue(new Document { Id = i });
                }
            }

            Task.Delay(10000).Wait();

            queue.Dispose();

            Console.ReadKey();
        }
    }
}
