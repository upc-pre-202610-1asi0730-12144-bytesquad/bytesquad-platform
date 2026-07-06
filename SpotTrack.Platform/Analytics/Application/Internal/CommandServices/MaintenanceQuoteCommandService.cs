using SpotTrack.Platform.Analytics.Application.CommandServices;
using SpotTrack.Platform.Analytics.Domain.Model;
using SpotTrack.Platform.Analytics.Domain.Model.Aggregates;
using SpotTrack.Platform.Analytics.Domain.Model.Commands;
using SpotTrack.Platform.Analytics.Domain.Model.ValueObjects;
using SpotTrack.Platform.Analytics.Domain.Repositories;
using SpotTrack.Platform.Shared.Application.Model;

namespace SpotTrack.Platform.Analytics.Application.Internal.CommandServices;

public class MaintenanceQuoteCommandService : IMaintenanceQuoteCommandService
{
    private readonly IMaintenanceQuoteRepository _maintenanceQuoteRepository;

    public MaintenanceQuoteCommandService(IMaintenanceQuoteRepository maintenanceQuoteRepository)
    {
        _maintenanceQuoteRepository = maintenanceQuoteRepository;
    }

    public async Task<Result<MaintenanceQuote>> Handle(RequestCorrectiveActionsCostCommand command)
    {
        var maintenanceQuote = new MaintenanceQuote(command);
        await _maintenanceQuoteRepository.AddAsync(maintenanceQuote);
        maintenanceQuote.InitializeId();
        await _maintenanceQuoteRepository.UpdateAsync(maintenanceQuote);
        return Result<MaintenanceQuote>.Success(maintenanceQuote);
    }

    public async Task<Result<MaintenanceQuote>> Handle(RequestSparePartsCostCommand command)
    {
        var maintenanceQuoteId = new MaintenanceQuoteId(command.MaintenanceQuoteId);
        var maintenanceQuote = await _maintenanceQuoteRepository.FindByMaintenanceQuoteIdAsync(maintenanceQuoteId);

        if (maintenanceQuote == null) return Result<MaintenanceQuote>.Failure(AnalyticsError.NotFound, "Maintenance quote not found.");
        if (maintenanceQuote.AdminId != command.AuthenticatedAdminId) return Result<MaintenanceQuote>.Failure(AnalyticsError.Forbidden, "You do not have permission to modify this maintenance quote.");

        maintenanceQuote.UpdateSparePartsCost(command.SparePartsCost);
        await _maintenanceQuoteRepository.UpdateAsync(maintenanceQuote);
        return Result<MaintenanceQuote>.Success(maintenanceQuote);
    }

    public async Task<Result<MaintenanceQuote>> Handle(RequestPreventiveCostCommand command)
    {
        var maintenanceQuoteId = new MaintenanceQuoteId(command.MaintenanceQuoteId);
        var maintenanceQuote = await _maintenanceQuoteRepository.FindByMaintenanceQuoteIdAsync(maintenanceQuoteId);

        if (maintenanceQuote == null) return Result<MaintenanceQuote>.Failure(AnalyticsError.NotFound, "Maintenance quote not found.");
        if (maintenanceQuote.AdminId != command.AuthenticatedAdminId) return Result<MaintenanceQuote>.Failure(AnalyticsError.Forbidden, "You do not have permission to modify this maintenance quote.");

        maintenanceQuote.UpdatePreventiveCost(command.PreventiveCost);
        await _maintenanceQuoteRepository.UpdateAsync(maintenanceQuote);
        return Result<MaintenanceQuote>.Success(maintenanceQuote);
    }

    public async Task<Result<MaintenanceQuote>> Handle(RequestMaintenanceCostCommand command)
    {
        var maintenanceQuoteId = new MaintenanceQuoteId(command.MaintenanceQuoteId);
        var maintenanceQuote = await _maintenanceQuoteRepository.FindByMaintenanceQuoteIdAsync(maintenanceQuoteId);

        if (maintenanceQuote == null) return Result<MaintenanceQuote>.Failure(AnalyticsError.NotFound, "Maintenance quote not found.");
        if (maintenanceQuote.AdminId != command.AuthenticatedAdminId) return Result<MaintenanceQuote>.Failure(AnalyticsError.Forbidden, "You do not have permission to modify this maintenance quote.");

        maintenanceQuote.ConsolidateTotalMaintenanceCost();
        await _maintenanceQuoteRepository.UpdateAsync(maintenanceQuote);
        return Result<MaintenanceQuote>.Success(maintenanceQuote);
    }
}
