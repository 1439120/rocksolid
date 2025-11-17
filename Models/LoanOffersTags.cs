
using System.ComponentModel.DataAnnotations;
using Google.Cloud.Firestore;
namespace BlazorApp.Models
{
    [FirestoreData]
    public class LoanOffersTags{
        [FirestoreProperty, Required]
        public string Name {get;set;} = string.Empty;
        [FirestoreProperty, Required]
        public string LoanOfferId{get; set;} = string.Empty;
    }
}