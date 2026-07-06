namespace SpotTrack.Platform.Profiles.Domain.Model.Aggregates;

public class Business
{
    private Business() { }

    public Business(int adminId, string companyName, string ruc,
        string legalStructure, string companyPhone, string companyEmail)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(companyName);
        ArgumentException.ThrowIfNullOrWhiteSpace(ruc);
        ArgumentException.ThrowIfNullOrWhiteSpace(legalStructure);
        AdminId = adminId;
        CompanyName = companyName;
        Ruc = ruc;
        LegalStructure = legalStructure;
        CompanyPhone = companyPhone;
        CompanyEmail = companyEmail;
    }

    public int Id { get; private set; }
    public int AdminId { get; private set; }
    public string CompanyName { get; private set; } = null!;
    public string Ruc { get; private set; } = null!;
    public string LegalStructure { get; private set; } = null!;
    public string CompanyPhone { get; private set; } = null!;
    public string CompanyEmail { get; private set; } = null!;
}
