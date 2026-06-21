using System;
using SpotTrack.Platform.Analytics.Domain.Model.Commands;
using SpotTrack.Platform.Analytics.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Analytics.Domain.Model.Aggregates;

public class MaintenanceQuote
{
    public long Id { get; private set; }
    public MaintenanceQuoteId MaintenanceQuoteId { get; private set; }
    public double CorrectiveActionsCost { get; private set; }
    public double SparePartsCost { get; private set; }
    public double PreventiveCost { get; private set; }
    public double TotalMaintenanceCost { get; private set; }

    protected MaintenanceQuote() { }

    public MaintenanceQuote(RequestCorrectiveActionsCostCommand command)
    {
        MaintenanceQuoteId = new MaintenanceQuoteId(new Random().Next(1000, 100000));
        CorrectiveActionsCost = command.CorrectiveActionsCost;
        SparePartsCost = 0;
        PreventiveCost = 0;
        TotalMaintenanceCost = command.CorrectiveActionsCost;
    }

    // Método de negocio para la Feature 6
    public void UpdateSparePartsCost(double sparePartsCost)
    {
        SparePartsCost = sparePartsCost;
        TotalMaintenanceCost = CorrectiveActionsCost + SparePartsCost + PreventiveCost;
    }
    
    // Método de negocio para la Feature 7
    public void UpdatePreventiveCost(double preventiveCost)
    {
        PreventiveCost = preventiveCost;
        TotalMaintenanceCost = CorrectiveActionsCost + SparePartsCost + PreventiveCost;
    }

}