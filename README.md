# NiceQueue
C# .NET Abstraction for AMQP clients

Currently supporting: AmazonSQS, RabitMQ, FileQueue, MemoryQueue

## Usage

### Instantiate:

```
var queueService = WhateverIoCYouUse.Resolve<INiceQueue>();
```

### Enqueue:

```
var messageId = queueService.Enqueue<EmailMessage>("my-queue", myEmailMessage);
```

### Get:

```
queueService.Dequeue<EmailMessage>("my-queue", (response) => {
    var myEmail = response.Value;
    ...
    return true;
});
```

### Delete:

```
queueService.Delete("my-queue", "abcd-1234-fghi-5678-klmn");
```
