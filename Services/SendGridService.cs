using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using RecipeApp.Models;

namespace RecipeApp.Services
{
    public class SendGridService : IMailService
    {
        public SendGridService(IOptions<SendGridSettings> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public SendGridSettings Options { get; }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var client = new SendGridClient(Options.SendGridKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(Options.SendGridEmail, Options.SendGridUser),
                Subject = mailRequest.Subject,
                HtmlContent = mailRequest.Body
            };

            msg.AddTo(new EmailAddress(mailRequest.ToEmail));

            msg.SetClickTracking(false, false);
            
            await client.SendEmailAsync(msg);
        }
    }
}