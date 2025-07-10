using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Konscious.Security.Cryptography;
namespace task_management_system_api.Utils;
public class PasswordService : IPasswordService
{
    public byte[] GenerateSalt(int size = 16)
    {
        var salt = new byte[size];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }

    public async Task<byte[]> HashPasswordAsync(string password, byte[] salt)
    {
        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = 8,
            Iterations = 4,
            MemorySize = 1024 * 64 // 64 MB
        };

        return await argon2.GetBytesAsync(16); // 128-bit hash
    }

    public async Task<bool> VerifyPasswordAsync(string enteredPassword, byte[] storedHash, byte[] storedSalt)
    {
        var hashOfInput = await HashPasswordAsync(enteredPassword, storedSalt);
        return ConstantTimeComparison(hashOfInput, storedHash);
    }

    private bool ConstantTimeComparison(byte[] a, byte[] b)
    {
        if (a.Length != b.Length) return false;
        int diff = 0;
        for (int i = 0; i < a.Length; i++)
        {
            diff |= a[i] ^ b[i];
        }
        return diff == 0;
    }
}