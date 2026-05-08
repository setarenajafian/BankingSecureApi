using BCrypt.Net;

namespace BankingSecureApi.Services;

public interface IUserService
{
    string HashPassword(string password);
    bool VerifyPassword(string password,string HashedPassword);
}
public class UserService : IUserService //users management
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    public bool VerifyPassword(string password, string HashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, HashedPassword);
    }
}
