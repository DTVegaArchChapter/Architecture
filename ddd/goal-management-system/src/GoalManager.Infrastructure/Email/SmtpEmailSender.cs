using GoalManager.Core.Interfaces;

namespace GoalManager.Infrastructure.Email;

/// <summary>
/// MimeKit is recommended over this now:
/// https://weblogs.asp.net/sreejukg/system-net-mail-smtpclient-is-not-recommended-anymore-what-is-the-alternative
/// </summary>
public class SmtpEmailSender(ILogger<SmtpEmailSender> logger,
                       IOptions<MailserverConfiguration> mailserverOptions) : IEmailSender
{
  private readonly ILogger<SmtpEmailSender> _logger = logger;
  private readonly MailserverConfiguration _mailserverConfiguration = mailserverOptions.Value!;

  public async Task SendEmailAsync(string to, string from, string subject, string body)
  {
    var emailClient = new System.Net.Mail.SmtpClient(_mailserverConfiguration.Hostname, _mailserverConfiguration.Port);

    var message = new MailMessage
    {
      From = new MailAddress(from),
      Subject = subject,
      Body = body
    };
    message.To.Add(new MailAddress(to));
    await emailClient.SendMailAsync(message);

    _logger.LogWarning("Sending email to {To} from {From} with subject {Subject} using {Type}.", to, from, subject, this.ToString());
  }
}
