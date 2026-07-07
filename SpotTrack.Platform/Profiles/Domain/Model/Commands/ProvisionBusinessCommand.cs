namespace SpotTrack.Platform.Profiles.Domain.Model.Commands;

public record ProvisionBusinessCommand(
    int AdminId,
    string CompanyName,
    string Ruc,
    string LegalStructure,
    string CompanyPhone,
    string CompanyEmail);
