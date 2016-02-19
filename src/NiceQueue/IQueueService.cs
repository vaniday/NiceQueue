using System;

namespace NiceQueue
{
    public interface IQueueService
    {
        void Enqueue<T>(string queueName, T payload);
        void Dequeue<T>(string queueName, Func<T, bool> callback);
        void Delete(string queueName, string messageId);
    }
}