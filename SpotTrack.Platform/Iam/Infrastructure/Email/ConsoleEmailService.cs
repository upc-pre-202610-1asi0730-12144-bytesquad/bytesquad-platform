using Microsoft.Extensions.Logging;
using SpotTrack.Platform.Iam.Application.Internal.OutboundServices;

namespace SpotTrack.Platform.Iam.Infrastructure.Email;

public class ConsoleEmailService(ILogger<ConsoleEmailService> logger) : IEmailService
{
    public Task SendAsync(string to, string subject, string body)
    {
        logger.LogWarning("[EMAIL] To: {To} | Subject: {Subject} | Body: {Body}", to, subject, body);
        return Task.CompletedTask;
    }
}
