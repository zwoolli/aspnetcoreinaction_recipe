using System.Threading.Tasks;
using RecipeApp.Models;

namespace RecipeApp.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}