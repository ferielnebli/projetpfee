using DynoTools;
using Serilog;
using Server.Kafka;
using WatchDog;

LoggerConfiguration LogConfig = new LoggerConfiguration();


var builder = Host.CreateDefaultBuilder(args);

builder.UseSerilog();

builder.ConfigureServices((hostContext, services) =>
{
    KafkaConfig config;
    hostContext.ReadConfig("Kafka_BootstrapServer", out config);
    services.AddSingleton(config);

    LogstashConfig logstashConfig;
    hostContext.ReadConfig("Logstash_Configuration", out logstashConfig);
    services.AddSingleton(logstashConfig);

    Log.Logger = LogConfig.LoggerConfig(logstashConfig).CreateLogger();

    services.AddSingleton<IProducer<HeartBeatMessage>>(producer => new Producer<HeartBeatMessage>(Topic.TOPIC_WATCHDOG_SEND_MESSAGE, config));
    services.AddSingleton<IConsumer<HeartBeatMessage>>(consumer => new Consumer<HeartBeatMessage>(Topic.TOPIC_WATCHDOG_RECEIVE_MESSAGE, config));
    services.AddHostedService<WatchDogWorker>();
});

using var host = builder.Build();

host.Run();


