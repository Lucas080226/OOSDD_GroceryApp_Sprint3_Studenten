using Grocery.Core.Helpers;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Text.RegularExpressions;

namespace Grocery.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IClientService _clientService;

        public AuthService(IClientService clientService)
        {
            _clientService = clientService;
        }

        public Client? Login(string email, string password)
        {
            Client? client = _clientService.Get(email);
            if (client == null) return null;
            if (PasswordHelper.VerifyPassword(password, client.Password)) return client;
            return null;
        }

        public Client? Registratie(string email, string password, out string? errorMessage)
        {
            errorMessage = null;

            // 1️⃣ Check leeg e-mail
            if (string.IsNullOrWhiteSpace(email))
            {
                errorMessage = "Vul een e-mailadres in.";
                return null;
            }

            // 2️⃣ Simpele e-mail validatie
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                errorMessage = "Ongeldig e-mailadres.";
                return null;
            }

            // 3️⃣ Check of account al bestaat
            if (_clientService.Get(email) != null)
            {
                errorMessage = "Account bestaat al.";
                return null;
            }

            // 4️⃣ Hash wachtwoord en maak account
            string hashedPassword = PasswordHelper.HashPassword(password);
            Client? newClient = _clientService.Add(email, hashedPassword);

            if (newClient != null)
            {
                return newClient;
            }

            errorMessage = "Registratie mislukt.";
            return null;
        }
    }
}
