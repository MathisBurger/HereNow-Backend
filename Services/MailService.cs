using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace PresenceBackend.Services;

public class MailService
{
    private readonly SmtpOptions _smtpOptions;
    
    public MailService(IOptions<SmtpOptions> smtpOptions)
    {
        _smtpOptions = smtpOptions.Value;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            using var client = new SmtpClient(_smtpOptions.Host, _smtpOptions.Port)
            {
                Credentials = new NetworkCredential(_smtpOptions.UserName, _smtpOptions.Password),
                EnableSsl = _smtpOptions.EnableSSL
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpOptions.UserName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(to);

            await client.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}