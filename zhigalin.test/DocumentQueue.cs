using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using zhigalin.test.Connectors;
using zhigalin.test.Interfaces;
using zhigalin.test.Models;

namespace zhigalin.test
{
    class DocumentQueue : IDocumentQueue, IDisposable
    {
        private bool _disposed = false;
        private readonly CancellationTokenSource source = new CancellationTokenSource();
        private readonly Queue<Document> _queue = new Queue<Document>();
        private readonly ExternalSystemConnector _connector;
        private readonly CancellationToken _token;
        private readonly Task _task;

        public DocumentQueue(ExternalSystemConnector connector)
        {
            _token = source.Token;
            _connector = connector;

            _task = Task.Run(() => Start(_token));
        }

        public void Enqueue(Document document)
        {
            lock(_queue)
            {
                _queue.Enqueue(document);
            }
        }

        private async Task Start(CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Send();
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }

        private Task Send()
        {
            lock (_queue)
            {
                var tmp = new Queue<Document>();
                var count = _queue.Count > 10 ? 10 : _queue.Count;

                if (count == 0)
                {
                    return Task.CompletedTask;
                }

                for (int i = 0; i < count; i++)
                {
                    tmp.Enqueue(_queue.Dequeue());
                }
                return _connector.SendDocument(tmp);
            }
        }

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                source.Cancel();
                source.Dispose();
                _task.Dispose();
            }

            _disposed = true;
        }
    }
}
