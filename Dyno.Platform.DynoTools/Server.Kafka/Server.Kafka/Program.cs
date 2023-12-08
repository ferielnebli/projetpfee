using DynoTools;
using Server.Kafka;


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        KafkaConfig config;

        hostContext.ReadConfig("Kafka_BootstrapServer", out config);

        services.AddSingleton(config);

        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
