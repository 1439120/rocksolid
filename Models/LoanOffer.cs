
using System.ComponentModel.DataAnnotations;
using Google.Cloud.Firestore;
namespace BlazorApp.Models
{
    [FirestoreData]
    public class LoanOffer{
        [FirestoreProperty]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [FirestoreProperty, Required(ErrorMessage = "Please write the title of the loan.")]
        public string Name {get;set;} = string.Empty;
        [FirestoreProperty]
        public float MinimumAmount{get; set;}
        [FirestoreProperty, Required(ErrorMessage = "The maximum amount for the loan is required")]
        public float MaximumAmount{get; set;}
        [FirestoreProperty, Required(ErrorMessage = "The minimum interest for the loan is required")]
        public float MinimumInterest{get; set;}
        [FirestoreProperty, Required(ErrorMessage = "The maximum interest for the loan is required")]
        public float MaximumInterest{get; set;}
        [FirestoreProperty]
        public LoanTypes LoanType{get; set;} = LoanTypes.None;
        [FirestoreProperty]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [FirestoreProperty]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public List<LoanOffersTags> loanOffersTags{get; set;} = [];

    }


    // public enum 
}