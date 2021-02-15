using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using zhigalin.test.Models;

namespace zhigalin.test.Connectors
{
    public sealed class ExternalSystemConnector
    {
        public async Task SendDocument(IReadOnlyCollection<Document> documents)
        {
            if (documents.Count > 10)
            {
                throw new ArgumentException("Can't send more than 10 documents at once.", nameof(documents));
            }

            await Task.Delay(TimeSpan.FromSeconds(2));
        }
    }
}
