﻿using Hangfire;
using RecallerBot.Constants;
using RecallerBot.Filters;
using RecallerBot.Models;
using RecallerBot.Models.Configuration;
using RecallerBot.Services;
using Telegram.Bot;

namespace RecallerBot.Extensions;

internal static class WebApplicationExtensions
{
    public static void AddHangfireDashboard(this WebApplication webApp)
    {
        HangfireDashboardAccess dashboardAccess = webApp.Configuration
                                    .GetSection(nameof(HangfireDashboardAccess))
                                    .Get<HangfireDashboardAccess>()!;

        webApp
            .UseHangfireDashboard(options: new DashboardOptions()
            {
                DashboardTitle = "Jobs condition",
                Authorization = new[] { new DashboardReadAuthorizationFilter(dashboardAccess) }
            });
    }

    public static void AddPost(this WebApplication webApp, Bot configuration) =>
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
            .ExcludeFromDescription()
            .AllowAnonymous();
}
