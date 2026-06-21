using SpotTrack.Platform.Analytics.Application.CommandServices;
using SpotTrack.Platform.Analytics.Domain.Model.Aggregates;
using SpotTrack.Platform.Analytics.Domain.Model.Commands;
using SpotTrack.Platform.Analytics.Domain.Repositories;

namespace SpotTrack.Platform.Analytics.Application.Internal.CommandServices;

public class MaintenanceQuoteCommandService : IMaintenanceQuoteCommandService
{
    private readonly IMaintenanceQuoteRepository _maintenanceQuoteRepository;

    public MaintenanceQuoteCommandService(IMaintenanceQuoteRepository maintenanceQuoteRepository)
    {
        _maintenanceQuoteRepository = maintenanceQuoteRepository;
    }

    public async Task<MaintenanceQuote?> Handle(RequestCorrectiveActionsCostCommand command)
    {
        var maintenanceQuote = new MaintenanceQuote(command);
        await _maintenanceQuoteRepository.AddAsync(maintenanceQuote);
        return maintenanceQuote;
    }
}