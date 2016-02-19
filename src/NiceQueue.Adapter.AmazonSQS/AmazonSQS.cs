using System;
using System.Net;
using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;
using Microsoft.Extensions.OptionsModel;

namespace NiceQueue.Adapter.AmazonSQS
{
    public class AmazonSQS : IQueueService
    {
        AmazonSQSClient Client;
        AmazonSQSConfig ClientConfig;
        AmazonSQSClientConfig Config;

        public AmazonSQS(IOptions<AmazonSQSClientConfig> config)
        {
            Config = config.Value;
            
            ClientConfig = new AmazonSQSConfig
            {
                RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(Config.Region) ?? null
            };

            if (Config.HasCredentials) {
                Client = new AmazonSQSClient(new Amazon.Runtime.BasicAWSCredentials(Config.AccessKey, Config.SecretKey), ClientConfig);
            } else {
                Client = new AmazonSQSClient(ClientConfig);
            }
        }

        public AmazonSQS ()
        {
            ClientConfig = new Amazon.SQS.AmazonSQSConfig();
            Client = new AmazonSQSClient(ClientConfig);
        }

        protected string GetQueueUrl(string queueName)
        {
            var result = Client.GetQueueUrl(queueName);
            
            if (result.HttpStatusCode != HttpStatusCode.OK) {
                throw new Exception(String.Format("Error while trying to get queue url. Status code: {0}", result.HttpStatusCode));
            }
            
            return result.QueueUrl;
        }

        public void Enqueue<T>(string queueName, T payload)
        {
            var result = Client.SendMessage(GetQueueUrl(queueName), typeof(T) == typeof(string) ? (string)(object)payload : JsonConvert.SerializeObject(payload));
            
            if (result.HttpStatusCode != HttpStatusCode.OK) {
                throw new CouldNotDeleteMessageException(String.Format("Status code: {0}", result.HttpStatusCode));
            }
        }

        public void Dequeue<T>(string queueName, Func<T, bool> callback)
        {
            var result = Client.ReceiveMessage(new ReceiveMessageRequest
                {
                    QueueUrl = GetQueueUrl(queueName),
                    MaxNumberOfMessages = 10
                });

            if (result.HttpStatusCode == HttpStatusCode.OK) {
                dynamic returnValue;
                
                foreach (var message in result.Messages) {
                    if (typeof(T) == typeof(string)) {
                        returnValue = message.Body;
                    } else {
                        returnValue = JsonConvert.DeserializeObject<T>(message.Body);
                    }

                    if (callback(returnValue)) {
                        Delete(queueName, message.ReceiptHandle);
                    }
                }
            } else {
                throw new CouldNotFetchMessageException(String.Format("Status code: {0}", result.HttpStatusCode));
            }
        }

        public void Delete(string queueName, string receiptHandle)
        {
            var result = Client.DeleteMessage(GetQueueUrl(queueName), receiptHandle);

            if (result.HttpStatusCode != HttpStatusCode.OK) {
                throw new CouldNotDeleteMessageException(String.Format("Status code: {0}", result.HttpStatusCode));
            }
        }
    }
}
