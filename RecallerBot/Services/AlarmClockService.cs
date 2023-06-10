﻿using RecallerBot.Constants;
using RecallerBot.Enums;
using RecallerBot.Models.Schedule;

namespace RecallerBot.Services;

internal sealed class AlarmClockService
{
    private readonly ScheduleService _scheduleService;
    private readonly ChatMessageService _chatMessageService;

    public AlarmClockService(ScheduleService scheduleService, ChatMessageService chatMessageService)
    {
        _scheduleService = scheduleService;
        _chatMessageService = chatMessageService;
    }

    public void SetUp()
    {
        _scheduleService.Schedule<BotRequestService>(new Job(
            message: NotificationMessages.AlarmClockReminder,
            chatId: _chatMessageService.TestChat,
            triggerTime: new Time
            {
                TimePeriod = TimePeriod.MimuteInterval,
                MinuteInterval = 15
            }));
    }
}
