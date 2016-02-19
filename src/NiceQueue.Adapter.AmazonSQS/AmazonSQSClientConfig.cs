namespace NiceQueue.Adapter.AmazonSQS
{
    public class AmazonSQSClientConfig
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string Region { get; set; }
        
        public bool HasCredentials { get { return !string.IsNullOrEmpty(AccessKey) && !string.IsNullOrEmpty(SecretKey); } }
    }
}
