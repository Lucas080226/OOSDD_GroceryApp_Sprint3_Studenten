
using Grocery.Core.Models;

namespace Grocery.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Client? Login(string email, string password);
        Client? Registratie(string email, string password, out string? errorMessage);
    }
}