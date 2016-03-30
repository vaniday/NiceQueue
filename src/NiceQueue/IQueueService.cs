using System;
using Newtonsoft.Json;

namespace NiceQueue
{
    public interface IQueueService
    {
        JsonSerializerSettings JsonSerializerSettings { get; set; }
        void Enqueue<T>(string queueName, T payload);
        bool Dequeue<T>(string queueName, Func<T, bool> callback);
        void Delete(string queueName, string messageId);
    }
}