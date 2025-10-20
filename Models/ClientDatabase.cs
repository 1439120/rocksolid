public class ClientDatabase{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name {get;set;} = string.Empty;
    public string Email {get; set;}= string.Empty;
    public int NumberOfApplications = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}