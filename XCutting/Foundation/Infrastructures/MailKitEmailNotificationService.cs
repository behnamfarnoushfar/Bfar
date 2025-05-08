using Bfar.XCutting.Abstractions.ApplicationServices;
using Bfar.XCutting.Abstractions.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using Polly;
using Polly.Retry;
using System;

namespace Bfar.XCutting.Foundation.Infrastructures
{

    public sealed class MailKitEmailNotificationService : INotificationService<NotificationModel, string>
    {
        private readonly EndPointConfigModel _smtpOptions;
        private readonly ILogger? logger;
        private readonly AsyncRetryPolicy _retryPolicy;
        public MailKitEmailNotificationService(EndPointConfigModel smtpOptions, ILogger logger)
        {
            _smtpOptions = smtpOptions;
            this.logger = logger;
            _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt * 2),
                (exception, timeSpan, retryCount, context) =>
                {
                    logger?.LogWarning("Retry {RetryCount}: Failed to send email. Waiting {TimeSpan} before next attempt. Error: {Message}",
                        retryCount, timeSpan, exception.Message);
                });

        }

        public async Task<Memory<byte>?> SendNotificationAsync(NotificationModel notification, CancellationToken cancellationToken = default)
        {
            if (notification?.Receipients is null || !notification.Receipients.Any())
                throw new ArgumentException("Recipients list cannot be empty.");


            return await _retryPolicy.ExecuteAsync(async () =>
            {
                notification.Id = Guid.NewGuid().ToString();
                using var smtp = new SmtpClient();
                smtp.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

                try
                {
                    await smtp.ConnectAsync(_smtpOptions.Host, _smtpOptions.Port, SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync(_smtpOptions.Username, _smtpOptions.Password);

                    var message = new MimeMessage
                    {
                        From = { new MailboxAddress(notification.Sender, notification.Sender) },
                        Subject = notification.Title
                    };

                    message.To.AddRange(notification.Receipients.Select(r => new MailboxAddress(r, r)));

                    var bodyBuilder = new BodyBuilder { TextBody = notification.Content };
                    if (notification.HasHtml) bodyBuilder.HtmlBody = notification.Content;

                    message.Body = bodyBuilder.ToMessageBody();

                    await smtp.SendAsync(message,cancellationToken);
                    await smtp.DisconnectAsync(true);

                    return new Memory<byte>(new byte[] { 1 });
                }
                catch (Exception ex)
                {

                    logger?.LogError(ex, notification.Receipients?.FirstOrDefault());
                    return null;
                }

            });
        }

        public async Task<decimal> GetRemainCreditAsync() => await Task.FromResult(long.MaxValue);
        public async Task<bool> GetDeliveryReportAsync(string id) => await Task.FromResult(true);

        public Task<Memory<byte>?> QueueNotificationAsync(NotificationModel notification)
        {
            throw new NotImplementedException();
        }
    }
}
