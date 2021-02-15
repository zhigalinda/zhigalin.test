using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using zhigalin.test.Connectors;
using zhigalin.test.Interfaces;
using zhigalin.test.Models;

namespace zhigalin.test
{
    class DocumentQueue : IDocumentQueue
    {
        public static bool Stop { get; set; }
        private static Queue<Document> queue;
        private static Thread thread;
        private static readonly ExternalSystemConnector connector;

        static DocumentQueue()
        {
            queue = new Queue<Document>();
            connector = new ExternalSystemConnector();
            Stop = false;
        }

        public DocumentQueue()
        {            
            if (thread == null)
            {
                thread = new Thread(Start);
                thread.Start();
            }
        }

        public void Enqueue(Document document)
        {
            lock(queue)
            {
                queue.Enqueue(document);
            }
        }

        private void Start()
        {
            while (true)
            {
                if (!Stop)
                {
                    Send();
                }
                Task.Delay(1000).Wait();
            }
        }

        private Task Send()
        {
            lock (queue)
            {
                var tmp = new Queue<Document>();
                var count = queue.Count > 10 ? 10 : queue.Count;

                for (int i = 0; i < count; i++)
                {
                    tmp.Enqueue(queue.Dequeue());
                }
                return connector.SendDocument(tmp);
            }
        }
    }
}
