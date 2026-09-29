using System.Net;
using System.Net.Mail;
using FormManagementSystem.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FormManagementSystem.Services;

public class SmtpEmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<SmtpEmailService> _logger;

    public SmtpEmailService(IOptions<EmailSettings> emailSettings, ILogger<SmtpEmailService> logger)
    {
        _emailSettings = emailSettings.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        if (string.IsNullOrWhiteSpace(to))
        {
            throw new ArgumentException("Recipient email address is required.", nameof(to));
        }

        if (string.IsNullOrWhiteSpace(_emailSettings.FromEmail))
        {
            throw new InvalidOperationException("Email:FromEmail is not configured.");
        }

        var fromAddress = string.IsNullOrWhiteSpace(_emailSettings.FromName)
            ? new MailAddress(_emailSettings.FromEmail)
            : new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName);

        using var message = new MailMessage
        {
            From = fromAddress,
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        message.To.Add(new MailAddress(to));

        using var client = new SmtpClient(_emailSettings.Host, _emailSettings.Port)
        {
            EnableSsl = _emailSettings.EnableSsl
        };

        if (!string.IsNullOrWhiteSpace(_emailSettings.Username) && !string.IsNullOrWhiteSpace(_emailSettings.Password))
        {
            client.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
        }

        _logger.LogInformation("Sending email to {Recipient} with subject: {Subject} via SMTP {Host}:{Port}", to, subject, _emailSettings.Host, _emailSettings.Port);

        await client.SendMailAsync(message);
    }
}
