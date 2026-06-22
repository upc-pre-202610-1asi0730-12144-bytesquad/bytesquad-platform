namespace SpotTrack.Platform.Iam.Interfaces.Acl;

public interface IIamContextFacade
{
    Task<int> CreateUserAsync(string username, string password, string role);
    Task<int> FetchUserIdByUsernameAsync(string username);
    Task<string> FetchUsernameByUserIdAsync(int userId);
}
