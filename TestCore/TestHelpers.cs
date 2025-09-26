using Grocery.Core.Helpers;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using Grocery.Core.Services;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace TestCore
{
    // Fake IClientService voor testen
    public class FakeClientService : IClientService
    {
        private readonly List<Client> _clients = new();

        public Client? Get(string email) => _clients.FirstOrDefault(c => c.EmailAddress == email);
        public Client? Get(int id) => _clients.FirstOrDefault(c => c.Id == id);
        public List<Client> GetAll() => _clients.ToList();

        public Client? Add(string email, string password)
        {
            if (_clients.Any(c => c.EmailAddress == email) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return null;

            var newClient = new Client(_clients.Count + 1, email.Split('@')[0], email, password);
            _clients.Add(newClient);
            return newClient;
        }
    }

    [TestFixture]
    public class AuthServiceTests
    {
        private AuthService _authService = null!;
        private FakeClientService _fakeClientService = null!;

        [SetUp]
        public void Setup()
        {
            _fakeClientService = new FakeClientService();
            _authService = new AuthService(_fakeClientService);
        }

        // =====================
        // Registratie Tests
        // =====================

        [Test]
        public void Registratie_ShouldReturnClient_WhenValidEmailAndPassword()
        {
            // Arrange
            string email = "newuser@test.com";
            string password = "password123";

            // Act
            var client = _authService.Registratie(email, password, out string? errorMessage);

            // Assert
            Assert.IsNotNull(client);
            Assert.AreEqual(email, client!.EmailAddress);
            Assert.IsNull(errorMessage);
        }

        [Test]
        public void Registratie_ShouldReturnNull_WhenEmailIsEmpty()
        {
            // Arrange
            string email = "";
            string password = "password123";

            // Act
            var client = _authService.Registratie(email, password, out string? errorMessage);

            // Assert
            Assert.IsNull(client);
            Assert.AreEqual("Vul een e-mailadres in.", errorMessage);
        }

        [Test]
        public void Registratie_ShouldReturnNull_WhenEmailIsInvalid()
        {
            // Arrange
            string email = "invalid-email";
            string password = "password123";

            // Act
            var client = _authService.Registratie(email, password, out string? errorMessage);

            // Assert
            Assert.IsNull(client);
            Assert.AreEqual("Ongeldig e-mailadres.", errorMessage);
        }

        [Test]
        public void Registratie_ShouldReturnNull_WhenEmailAlreadyExists()
        {
            // Arrange
            string email = "existing@test.com";
            string password = "password123";

            // Voeg een bestaand account toe
            _fakeClientService.Add(email, "hashedpassword");

            // Act
            var client = _authService.Registratie(email, password, out string? errorMessage);

            // Assert
            Assert.IsNull(client);
            Assert.AreEqual("Account bestaat al.", errorMessage);
        }

        // =====================
        // PasswordHelper Tests
        // =====================

        [Test]
        public void TestPasswordHelperReturnsTrue()
        {
            // Arrange
            string password = "user3";
            string passwordHash = "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA=";

            // Act
            bool result = PasswordHelper.VerifyPassword(password, passwordHash);

            // Assert
            Assert.IsTrue(result);
        }

        [TestCase("user1", "IunRhDKa+fWo8+4/Qfj7Pg==.kDxZnUQHCZun6gLIE6d9oeULLRIuRmxmH2QKJv2IM08=")]
        [TestCase("user3", "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA=")]
        public void TestPasswordHelperReturnsTrue(string password, string passwordHash)
        {
            // Arrange & Act
            bool result = PasswordHelper.VerifyPassword(password, passwordHash);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void TestPasswordHelperReturnsFalse()
        {
            // Arrange
            string password = "wrongPassword";
            string passwordHash = "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA=";

            // Act
            bool result = PasswordHelper.VerifyPassword(password, passwordHash);

            // Assert
            Assert.IsFalse(result);
        }

        [TestCase("user1", "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA=")]
        [TestCase("user3", "IunRhDKa+fWo8+4/Qfj7Pg==.kDxZnUQHCZun6gLIE6d9oeULLRIuRmxmH2QKJv2IM08=")]
        public void TestPasswordHelperReturnsFalse(string password, string passwordHash)
        {
            // Arrange & Act
            bool result = PasswordHelper.VerifyPassword(password, passwordHash);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
