using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public Client? Get(string email)
        {
            return _clientRepository.Get(email);
        }

        public Client? Get(int id)
        {
            return _clientRepository.Get(id);
        }

        public List<Client> GetAll()
        {
            return _clientRepository.GetAll();
        }

        public Client? Add(string email, string password)
        {
            // Validation: Check if email already exists
            if (_clientRepository.EmailExists(email))
            {
                return null; // Email already in use
            }

            // Validation: Basic input checks
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return null; // Invalid input
            }

            // Business logic: Generate new ID
            var allClients = _clientRepository.GetAll();
            int newId = allClients.Any() ? allClients.Max(c => c.Id) + 1 : 1;

            // Business logic: Generate name from email
            string name = email.Split('@')[0];

            // Create the new client
            Client newClient = new Client(newId, name, email, password);

            // Use repository to save
            return _clientRepository.Add(newClient);
        }
    }
}