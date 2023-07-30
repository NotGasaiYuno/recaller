﻿using Hangfire.Dashboard;
using RecallerBot.Models.Configuration;
using System.Linq;

namespace RecallerBot.Filters;

public sealed class DashboardReadAuthorizationFilter : IDashboardAuthorizationFilter
{
    private readonly HangfireDashboardAccess _hangfireAccess;
    private readonly ILogger<DashboardReadAuthorizationFilter> _logger;

    public DashboardReadAuthorizationFilter(HangfireDashboardAccess hangfireAccess,
        ILogger<DashboardReadAuthorizationFilter> logger)
    {
        _hangfireAccess = hangfireAccess;
        _logger = logger;
    }

    public bool Authorize(DashboardContext context)
    {
        HttpContext httpContext = context.GetHttpContext();

        //var claim = httpContext.User.Claims.FirstOrDefault(x => x.Type == _hangfireAccess.ClaimName);

        _logger.LogInformation($"Azure AD access token {_hangfireAccess.ClaimType}: " + httpContext.Request.Headers[_hangfireAccess.ClaimType]);

        string claims = string.Empty;

        httpContext.User.Claims.ToList().ForEach(claim => claims += $"\n({claim.Type}, {claim.Value})\n");

        _logger.LogInformation("All claims: " + claims);

        return httpContext.User.Identity?.IsAuthenticated ?? false
            && httpContext.User.Claims.Any(claim =>
                {
                    return claim.Type == _hangfireAccess.ClaimType
                        && claim.Value == _hangfireAccess.ClaimValue;
                });
    }
}