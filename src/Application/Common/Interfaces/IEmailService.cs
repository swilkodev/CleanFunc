using System.Threading.Tasks;

namespace CleanFunc.Application.Common.Interfaces
{
    public interface IEmailService
    {
         Task SendAsync(EmailMessageDto message);
    }
}