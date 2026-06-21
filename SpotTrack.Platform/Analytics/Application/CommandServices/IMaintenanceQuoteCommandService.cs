using SpotTrack.Platform.Analytics.Domain.Model.Aggregates;
using SpotTrack.Platform.Analytics.Domain.Model.Commands;

namespace SpotTrack.Platform.Analytics.Application.CommandServices;

public interface IMaintenanceQuoteCommandService
{
    Task<MaintenanceQuote?> Handle(RequestCorrectiveActionsCostCommand command);
    
    Task<MaintenanceQuote?> Handle(RequestSparePartsCostCommand command);

}