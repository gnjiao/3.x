using System.Collections.Concurrent;

namespace Core.Collections.Concurrent
{
    public static class ConcurrentQueueExtensions
    {
        public static void Clear<T>(this ConcurrentQueue<T> queue)
        {
            T t;
            while (queue.TryDequeue(out t))
            {

            }
        }
    }
}