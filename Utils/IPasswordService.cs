namespace task_management_system_api.Utils;
public interface IPasswordService
{
    byte[] GenerateSalt(int size = 16);
    Task<byte[]> HashPasswordAsync(string password, byte[] salt);
    Task<bool> VerifyPasswordAsync(string enteredPassword, byte[] storedHash, byte[] storedSalt);

}