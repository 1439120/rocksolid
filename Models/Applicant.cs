
using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Models
{
    public class Applicant{
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required(ErrorMessage = "User Name is required.")]
        public string? Username { get; set; }
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string? Email { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        [Range(500, 20000, ErrorMessage = "The requested amount should be above R500.00.")]
        public float RequestedAmount {get;set;}
        public string Status {get; set;} = "New";
        public string Reference{get;set;}= "";
        public string ClientId{get;set;}= "";
        // public float ApprovedAmount{get;set;}
        public int NumberOfPayments{get;set;}
        [Required(ErrorMessage = "Select payment terms.")]
        public string PaymentTerms{get;set;} = "";
        // private string paymentTerms="";
        // public string PaymentTerms
        // {
        //     get => paymentTerms;
        //     set
        //     {
        //         paymentTerms = value;
        //         NumberOfPayments = int.TryParse(value, out var n) ? n : 0;
        //         MonthlyRepayment = NumberOfPayments > 0 ? RequestedAmount / NumberOfPayments : 0;
        //     }
        // }
        // private void CalculateMonthlyAmount(){
        //     if(PaymentTerms.Length >= 0){
        //         NumberOfPayments = int.Parse(PaymentTerms);
        //         MonthlyRepayment = RequestedAmount / NumberOfPayments;

        //     }else{
        //         MonthlyRepayment = MonthlyRepayment + 1;
        //     }
        // }
        public DateTime StartDate{get;set;}
        public DateTime EndDate{get;set;}
        public float RemainingAmount{get;set;}
        public float LinkedInterest{get;set;}
        public float MonthlyRepayment{get;set;}
         public void Approve(){
            Status = "Approved";
        }
        
    }

   
}