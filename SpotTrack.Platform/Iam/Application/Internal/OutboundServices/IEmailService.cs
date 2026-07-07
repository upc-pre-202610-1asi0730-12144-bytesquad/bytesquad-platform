namespace SpotTrack.Platform.Iam.Application.Internal.OutboundServices;

public interface IEmailService
{
    Task SendAsync(string to, string subject, string body);
}
