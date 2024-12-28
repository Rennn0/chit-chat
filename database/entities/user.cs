using System.Security.Cryptography;
using System.Text;

namespace database.entities
{
    public class User : Entity
    {
        public User(string password, string username)
        {
            Password = GetHash(password);
            Username = username.Trim('\r', '\n', ' ');
        }

        public byte[] Password { get; set; }
        public string Username { get; set; }

        public static byte[] GetHash(string input) =>
            SHA256.HashData(Encoding.UTF8.GetBytes(input));

        public bool VerifyPassword(string password) => GetHash(password).SequenceEqual(Password);
    }
}
