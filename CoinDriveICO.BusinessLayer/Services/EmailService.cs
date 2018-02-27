using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;

namespace CoinDriveICO.BusinessLayer.Services
{
    public interface IEmailService
    {
        Task<bool> SendRegisterConfirmationMessageAsync(string userName, string email, string confirmationUrl);
        Task<bool> SendPasswordResetMessageAsync(string username, string email, string resetTokenUrl);
    }

    public class EmailService : IEmailService
    {
        public async Task<bool> SendRegisterConfirmationMessageAsync(string userName, string email, string confirmationUrl)
        {
            var messageBody = "Please confirm your newly registered account with token:\n" + confirmationUrl;
            var messageSubject = "Confirm your account";
            var result = await SendMessageAsync(email, messageSubject, messageBody);
            return result;
        }

        public async Task<bool> SendPasswordResetMessageAsync(string username, string email, string resetTokenUrl)
        {
            var messageBody =
                "You requested password reset for your account, proceed to following link to reset your password:\n" +
                resetTokenUrl;
            var messageSubject = "Password reset";
            var result = await SendMessageAsync(email, messageSubject, messageBody);
            return result;
        }

        private async Task<bool> SendMessageAsync(string reciever, string subject, string body)
        {
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("CoinDrive Support", "support@coindrive.xyz"));
                emailMessage.To.Add(new MailboxAddress("", reciever));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart(TextFormat.Html)
                {
                    Text = body
                };
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("vps4440.ua-hosting.company", 587, false);
                    await client.AuthenticateAsync("support@coindrive.xyz", "GmcAr9sEg0");
                    await client.SendAsync(emailMessage);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}