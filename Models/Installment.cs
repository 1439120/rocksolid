using Google.Cloud.Firestore;

namespace BlazorApp.Models
{
    [FirestoreData]
    public class Installment{
        [FirestoreProperty]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [FirestoreProperty]
        public string ApplicantId{ get; set; } = "";
        [FirestoreProperty]
        public float Amount{get; set;}
        [FirestoreProperty]
        public DateTime PaymentDate { get; set; } = DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
        [FirestoreProperty]
        public string Status { get; set; } = "Unpaid";
        [FirestoreProperty]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [FirestoreProperty]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}