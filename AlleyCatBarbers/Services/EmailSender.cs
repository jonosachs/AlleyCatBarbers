using Azure;
using Azure.Communication.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace AlleyCatBarbers.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailClient _emailClient;
        private readonly string _fromEmail;

        public EmailSender(IConfiguration configuration)
        {
            var connectionString = configuration["AzureCommunicationServices:ConnectionString"];
            _emailClient = new EmailClient(connectionString);
            _fromEmail = "DoNotReply@93152d6b-b12c-40af-8ea8-c53636afbabc.azurecomm.net";
        }

        public async Task<(bool EmailSent, string Message)> SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                EmailSendOperation emailSendOperation = await _emailClient.SendAsync(
                WaitUntil.Completed,
                senderAddress: _fromEmail,
                recipientAddress: email,
                subject: subject,
                htmlContent: htmlMessage,
                plainTextContent: htmlMessage);

                return (true, $"Email queued for delivery. Status = {emailSendOperation.Value.Status}");
            }
            catch (RequestFailedException ex)
            {
                return (false, $"Email send failed. ErrorCode: {ex.ErrorCode}");
            }
            catch (Exception ex)
            {
                return (false, $"Email send failed.");
            }
        }

        Task IEmailSender.SendEmailWithAttachmentAsync(string email, string subject, string message, byte[] attachmentBytes, string attachmentName)
        {
            throw new NotImplementedException();
        }
    }
}

