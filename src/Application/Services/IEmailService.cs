﻿namespace TodoApp.Application.Services;

public interface IEmailService
{
    Task SendEmail(string recipient, string subject, string body);
}