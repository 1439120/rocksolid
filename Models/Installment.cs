public class Installment{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string ApplicantId{ get; set; } = "";
    public int Amount{get; set;}
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}