﻿using Hangfire;

namespace RecallerBot.Activator;

internal class HangfireActivator : JobActivator
{
    private readonly IServiceProvider _serviceProvider;

    public HangfireActivator(IServiceProvider serviceProvider) =>
        _serviceProvider = serviceProvider;

    public override object? ActivateJob(Type type) =>
        _serviceProvider.GetService(type);
}
