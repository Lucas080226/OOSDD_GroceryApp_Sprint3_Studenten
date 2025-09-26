using Grocery.Core.Models;

namespace Grocery.Core.Interfaces.Repositories
{
    public interface IClientRepository
    {
        public Client? Get(string email);
        public Client? Get(int id);
        public List<Client> GetAll();
        public Client Add(Client client);           // Add this method
        public bool EmailExists(string email);     // Add this method
    }
}