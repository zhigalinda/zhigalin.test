using System;
using System.Threading;
using System.Threading.Tasks;
using zhigalin.test.Models;

namespace zhigalin.test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            new Thread(CreateDocuments).Start();

            Task.Delay(2000).Wait();

            new Thread(CreateDocuments).Start();

            Task.Delay(2000).Wait();

            new Thread(CreateDocuments).Start();

            Task.Delay(2000).Wait();

            new Thread(Stop).Start();

            Task.Delay(2000).Wait();

            new Thread(Start).Start();
        }

        //Запуск отправки
        public static void Start()
        {
            DocumentQueue.Stop = false;
        }

        //Остановка отправки
        public static void Stop()
        {
            DocumentQueue.Stop = true;
        }

        //Добавление случайного количества документов
        public static void CreateDocuments()
        {
            var queue = new DocumentQueue();

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < new Random().Next(1, 20); j++)
                {
                    queue.Enqueue(new Document { Id = i });
                }
            }
        }
    }
}
