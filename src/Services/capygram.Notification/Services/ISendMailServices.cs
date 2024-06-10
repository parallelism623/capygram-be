using capygram.Common.DTOs.User;

namespace capygram.Notification.Services
{
    public interface ISendMailServices
    {
        Task SendMailHtml(SendMailHtml mail);
    }
}
