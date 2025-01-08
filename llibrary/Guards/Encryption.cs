using System.Security.Cryptography;
using System.Text;
using FileStream = System.IO.FileStream;

namespace LLibrary.Guards;

public class Encryption
{
    private const string _file = ".key";
    private const int _iterations = 20;

    public static void FlushOnDisk(string rawText, string encryptionKey)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(rawText);
        byte[] salt = new byte[16];
        Random r = new Random();
        r.NextBytes(salt);

        Rfc2898DeriveBytes passWordBytes =
            new(encryptionKey, salt, _iterations, HashAlgorithmName.SHA256);
        Aes encryptor = Aes.Create();

        encryptor.Key = passWordBytes.GetBytes(32);
        encryptor.IV = passWordBytes.GetBytes(16);

        using FileStream fs = new FileStream(_file, FileMode.CreateNew);
        fs.Write(salt, 0, salt.Length);

        using CryptoStream cs = new CryptoStream(
            fs,
            encryptor.CreateEncryptor(),
            CryptoStreamMode.Write
        );
        cs.Write(bytes, 0, bytes.Length);
    }

    public static string ReadFromDisk(string encryptionKey)
    {
        if (!File.Exists(_file))
            throw new FileNotFoundException(_file);

        using FileStream fs = new FileStream(_file, FileMode.Open);
        byte[] salt = new byte[16];
        fs.ReadExactly(salt, 0, salt.Length);

        using Rfc2898DeriveBytes passwordBytes =
            new(encryptionKey, salt, _iterations, HashAlgorithmName.SHA256);
        using Aes aes = Aes.Create();
        aes.Key = passwordBytes.GetBytes(32);
        aes.IV = passwordBytes.GetBytes(16);

        using CryptoStream cs = new CryptoStream(fs, aes.CreateDecryptor(), CryptoStreamMode.Read);
        using MemoryStream ms = new MemoryStream();
        cs.CopyTo(ms);
        byte[] decrypted = ms.ToArray();
        return Encoding.UTF8.GetString(decrypted);
    }
}
