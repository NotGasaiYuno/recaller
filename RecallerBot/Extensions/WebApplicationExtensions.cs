﻿using Hangfire;
using RecallerBot.Constants;
using RecallerBot.Models;
using RecallerBot.Services;
using Telegram.Bot;

namespace RecallerBot.Extensions;

internal static class WebApplicationExtensions
{
    public static void AddHangfireDashboard(this WebApplication webApp)
    {
        webApp.UseHangfireDashboard();
        webApp.MapHangfireDashboard();
    }

    public static void AddPost(this WebApplication webApp, BotConfiguration configuration) =>
        webApp
            .MapPost($"/bot/{configuration.EscapedBotToken}",
                    async (ITelegramBotClient botClient,
                            HttpRequest request,
                            HandleUpdateService handleUpdateService,
                            JsonUpdate update) =>
                    {
                        await handleUpdateService.StartAsync(update);

                        return Results.Ok();
                    })
            .WithName(WebhookConstants.Name)
            .ExcludeFromDescription();
}
