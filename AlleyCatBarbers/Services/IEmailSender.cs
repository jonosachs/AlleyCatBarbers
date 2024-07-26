﻿

namespace AlleyCatBarbers.Services
{
    public interface IEmailSender
    {
        Task<(bool EmailSent, string Message)> SendEmailAsync(string email, string subject, string message);
        Task SendEmailWithAttachmentAsync(string email, string subject, string message, byte[] attachmentBytes, string attachmentName);
    }
}
