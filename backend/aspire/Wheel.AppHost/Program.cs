var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedisContainer("redis", 6379);
var mq = builder.AddRabbitMQContainer("rabbitmq");

builder.AddProject<Projects.Wheel_WebApi_Host>("wheel.webapi.host")
    .WithReference(redis, "Redis")
    .WithReference(mq, "RabbitMq");
    ;

builder.Build().Run();
