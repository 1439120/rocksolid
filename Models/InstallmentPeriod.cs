using Google.Cloud.Firestore;


namespace BlazorApp.Models
{
    [FirestoreData]
    class InstallmentPeriod{
        [FirestoreProperty]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [FirestoreProperty]
        public string Name{get; set;} = string.Empty;
        [FirestoreProperty]
        public int NumberOfMonths{get; set;} = 0;
        [FirestoreProperty]
        public LoanTypes loanType{get; set;}
    }
}