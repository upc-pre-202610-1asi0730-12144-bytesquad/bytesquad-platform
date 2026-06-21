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
    
    public async Task<MaintenanceQuote?> Handle(RequestSparePartsCostCommand command)
    {
        var maintenanceQuoteId = new MaintenanceQuoteId(command.MaintenanceQuoteId);
        var maintenanceQuote = await _maintenanceQuoteRepository.FindByMaintenanceQuoteIdAsync(maintenanceQuoteId);
        
        if (maintenanceQuote == null) return null;

        maintenanceQuote.UpdateSparePartsCost(command.SparePartsCost);
        await _maintenanceQuoteRepository.UpdateAsync(maintenanceQuote);
        return maintenanceQuote;
    }
    
    public async Task<MaintenanceQuote?> Handle(RequestPreventiveCostCommand command)
    {
        var maintenanceQuoteId = new MaintenanceQuoteId(command.MaintenanceQuoteId);
        var maintenanceQuote = await _maintenanceQuoteRepository.FindByMaintenanceQuoteIdAsync(maintenanceQuoteId);
        
        if (maintenanceQuote == null) return null;

        maintenanceQuote.UpdatePreventiveCost(command.PreventiveCost);
        await _maintenanceQuoteRepository.UpdateAsync(maintenanceQuote);
        return maintenanceQuote;
    }
    
    public async Task<MaintenanceQuote?> Handle(RequestMaintenanceCostCommand command)
    {
        var maintenanceQuoteId = new MaintenanceQuoteId(command.MaintenanceQuoteId);
        var maintenanceQuote = await _maintenanceQuoteRepository.FindByMaintenanceQuoteIdAsync(maintenanceQuoteId);
        
        if (maintenanceQuote == null) return null;

        maintenanceQuote.ConsolidateTotalMaintenanceCost();
        await _maintenanceQuoteRepository.UpdateAsync(maintenanceQuote);
        return maintenanceQuote;
    }



}