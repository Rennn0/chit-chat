using fileServerCs;

const string fileConsumerQRk = "files";

RabbitFileConsumer fileConsumer = new RabbitFileConsumer(
    queue: fileConsumerQRk,
    routingKey: fileConsumerQRk,
    settings: Settings.RabbitSettings
);
await fileConsumer.InitializeAsync();
fileConsumer.AttachCallback(Callbacks.SaveFileAsync);

Console.ReadKey();