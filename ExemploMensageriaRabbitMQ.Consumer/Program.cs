using ExemploMensageriaRabbitMQ.Consumer.Listeners;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder =  Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<ClienteListener>();

var host = builder.Build();
host.Run();