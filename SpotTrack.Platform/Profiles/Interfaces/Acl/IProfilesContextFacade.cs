namespace SpotTrack.Platform.Profiles.Interfaces.Acl;

public interface IProfilesContextFacade
{
    Task<int> FetchClientIdByEmailAsync(string email);
    Task<int> FetchAdminIdByEmailAsync(string email);

    Task<int> CreateClientAsync(int userId, string email, string firstName, string lastName, string phoneNumber,
        string dni);
    Task<int> CreateAdminAsync(int userId, string email, string firstName, string lastName, string phoneNumber,
        string dni);

    Task<int> RegisterClientAsync(int userId);
    Task<int> RegisterAdminAsync(int userId);
}
