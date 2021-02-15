using zhigalin.test.Models;

namespace zhigalin.test.Interfaces
{
    public interface IDocumentQueue
    {
        void Enqueue(Document document);
    }
}
