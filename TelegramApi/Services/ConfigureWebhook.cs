using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TelegramBotLibrary.Helpers;

namespace TelegramApi.Services
{
    public class ConfigureWebhook : IHostedService
    {
        private readonly IServiceProvider _services;
        private readonly BotConfigs _botConfig;

        public ConfigureWebhook(IServiceProvider serviceProvider,
                                IConfiguration configuration)
        {
            _services = serviceProvider;
            _botConfig = configuration.GetSection("BotConfigs").Get<BotConfigs>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _services.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
            // Configure custom endpoint per Telegram API recommendations:
            // https://core.telegram.org/bots/api#setwebhook
            // If you'd like to make sure that the Webhook request comes from Telegram, we recommend
            // using a secret path in the URL, e.g. https://www.example.com/<token>.
            // Since nobody else knows your bot's token, you can be pretty sure it's us.
            var webhookAddress = @$"{_botConfig.HostAddress}/bot/{_botConfig.BotToken}";
            await botClient.SetWebhookAsync(
                url: webhookAddress,
                allowedUpdates: Array.Empty<UpdateType>(),
                cancellationToken: cancellationToken);

            Log.Info(@$"Setting WebHook: {_botConfig.HostAddress}/bot/{_botConfig.BotToken}");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using var scope = _services.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            // Remove webhook upon app shutdown
            await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);

            Log.Info(@$"Deleting WebHook: {_botConfig.HostAddress}/bot/{_botConfig.BotToken}");
        }
    }
}
