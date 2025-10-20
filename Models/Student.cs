using Google.Cloud.Firestore;

namespace BlazorApp.Models
{
    [FirestoreData]
    public class Student
    {
        [FirestoreProperty]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [FirestoreProperty]
        public string FirstName { get; set; } = string.Empty;

        [FirestoreProperty]
        public string LastName { get; set; } = string.Empty;

        [FirestoreProperty]
        public string Email { get; set; } = string.Empty;

        [FirestoreProperty]
        public string Course { get; set; } = string.Empty;

        [FirestoreProperty]
        public int Year { get; set; } = 1;

        [FirestoreProperty]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [FirestoreProperty]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}