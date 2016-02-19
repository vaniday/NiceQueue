using System;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace NiceQueue.Adapter.MemoryQueue
{
    public class MemoryQueue : IQueueService
    {
        ConcurrentDictionary<string, ConcurrentQueue<string>> Queues;

        public MemoryQueue()
        {
            Queues = new ConcurrentDictionary<string, ConcurrentQueue<string>>();
        }

        protected ConcurrentQueue<string> GetQueue(string queueName)
        {
            if (!Queues.ContainsKey(queueName))
                Queues[queueName] = new ConcurrentQueue<string>();

            return Queues[queueName];
        }

        public void Enqueue<T>(string queueName, T payload)
        {
            var queue = GetQueue(queueName);

            queue.Enqueue(typeof(T) == typeof(string) ? (string)(object)payload : JsonConvert.SerializeObject(payload));
        }

        public void Dequeue<T>(string queueName, Func<T, bool> callback)
        {
            var queue = GetQueue(queueName);

            string result;
            queue.TryDequeue(out result);
            
            dynamic returnValue;
            
            if (typeof(T) == typeof(string)) {
                returnValue = result;
            } else {
                returnValue = JsonConvert.DeserializeObject<T>(result);
            }

            if (!callback(returnValue)) {
                Enqueue<T>(queueName, returnValue);
            }
        }

        public void Delete(string queueName, string receiptHandle)
        {
            throw new NotImplementedException();
        }
    }
}
