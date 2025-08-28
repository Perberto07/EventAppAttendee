using System.Net;
using System.Net.Mail;

public class EmailService
{
    private readonly IConfiguration _config;
    public EmailService(IConfiguration config) => _config = config;

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        using var smtp = new SmtpClient(_config["Email:SmtpHost"], int.Parse(_config["Email:SmtpPort"]))
        {
            Credentials = new NetworkCredential(_config["Email:Username"], _config["Email:Password"]),
            EnableSsl = true
        };

        var message = new MailMessage(_config["Email:From"], to, subject, body)
        {
            IsBodyHtml = true
        };

        await smtp.SendMailAsync(message);
    }
}
