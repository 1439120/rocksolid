
using System.ComponentModel.DataAnnotations;
using Google.Cloud.Firestore;

namespace BlazorApp.Models
{
    [FirestoreData]
    public class Applicant{
        [FirestoreProperty]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [FirestoreProperty]
        public string? Username => Name +" "+ Surname;
        [FirestoreProperty]
        [Required(ErrorMessage = "User Name is required.")]
        public string Name { get; set; } = string.Empty;
        [FirestoreProperty]
        [Required(ErrorMessage = "User Surname is required.")]
        public string Surname { get; set; } = string.Empty;
        [FirestoreProperty]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string? Email { get; set; }
        [FirestoreProperty]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [FirestoreProperty]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        [FirestoreProperty]
        [Range(500, 20000, ErrorMessage = "The requested amount should be above R500.00.")]
        public float RequestedAmount {get;set;}
        [FirestoreProperty]
        public string Status {get; set;} = "New";
        [FirestoreProperty]
        public string Reference{get;set;}= $"LOAN/{DateTime.UtcNow:yyyyMMdd}/{Guid.NewGuid().ToString("N")[..4].ToUpper()}";
        [FirestoreProperty]
        public string ClientId{get;set;}= "";
        [FirestoreProperty]
        public string UserId{get;set;}= "";
        // public float ApprovedAmount{get;set;}
        [FirestoreProperty]
        public int NumberOfPayments{get;set;}
        [FirestoreProperty]
        [Required(ErrorMessage = "Select payment terms.")]
        public string PaymentTerms{get;set;} = "";
        [FirestoreProperty]
        public DateTime StartDate{get;set;} = DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
        [FirestoreProperty]
        public DateTime EndDate{get;set;} = DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
        [FirestoreProperty]
        public DateTime ApprovedDate{get;set;} = DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
        [FirestoreProperty]
        public float RemainingAmount{get;set;}
        [FirestoreProperty]
        public float LinkedInterest{get;set;}
        [FirestoreProperty]
        public float MonthlyRepayment{get;set;}
         public void Approve(){
            Status = "Approved";
            ApprovedDate = DateTime.UtcNow;
        }
        
    }

   
}