namespace BuildingBlocks.Infrastructure.RabbitMQInfra;

using BuildingBlocks.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

public class RabbitMQOptionsSetup(
    IConfiguration _configuration) : IConfigureOptions<RabbitMQOptions>
{
    public void Configure(RabbitMQOptions options)
    {
        _configuration.GetSection(RabbitMQOptions.SectionName).Bind(options);

        options.Port = SettingsUtils.GetEnvironmentValueOrDefault("RABBITMQ_PORT", options.Port);
        options.Host = SettingsUtils.GetEnvironmentValueOrDefault("RABBITMQ_HOST", options.Host);
        options.Username = SettingsUtils.GetEnvironmentValueOrDefault("RABBITMQ_USERNAME", options.Username);
        options.Password = SettingsUtils.GetEnvironmentValueOrDefault("RABBITMQ_PASSWORD", options.Password);
    }
}

public class RabbitMQOptions
{
    public const string SectionName = "RabbitMQOptions";

    public int Port { get; set; }
    public string Host { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
