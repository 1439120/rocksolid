
namespace BlazorApp.Models
{
    public class Applicant{
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public float RequestedAmount {get;set;}
        public string Status {get; set;} = "New";
        public string Reference{get;set;}= "";
        public string ClientId{get;set;}= "";
        // public float ApprovedAmount{get;set;}
        public int NumberOfPayments{get;set;}
        public DateTime StartDate{get;set;}
        public DateTime EndDate{get;set;}
        public float RemainingAmount{get;set;}

         public void Approve(){
            Status = "Approved";
        }
        
    }

   
}