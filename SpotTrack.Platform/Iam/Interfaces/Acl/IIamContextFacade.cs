namespace SpotTrack.Platform.Iam.Interfaces.Acl;

public interface IIamContextFacade
{
    Task<int> CreateUserAsync(string username, string password);
    Task<int> FetchUserIdByUsernameAsync(string username);
    Task<string> FetchUsernameByUserIdAsync(int userId);
}
