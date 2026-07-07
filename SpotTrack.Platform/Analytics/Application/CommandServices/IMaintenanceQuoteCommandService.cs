using SpotTrack.Platform.Analytics.Domain.Model.Aggregates;
using SpotTrack.Platform.Analytics.Domain.Model.Commands;
using SpotTrack.Platform.Shared.Application.Model;

namespace SpotTrack.Platform.Analytics.Application.CommandServices;

public interface IMaintenanceQuoteCommandService
{
    Task<Result<MaintenanceQuote>> Handle(RequestCorrectiveActionsCostCommand command);

    Task<Result<MaintenanceQuote>> Handle(RequestSparePartsCostCommand command);

    Task<Result<MaintenanceQuote>> Handle(RequestPreventiveCostCommand command);

    Task<Result<MaintenanceQuote>> Handle(RequestMaintenanceCostCommand command);
}
