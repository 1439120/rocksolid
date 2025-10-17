using Google.Cloud.Firestore;

namespace BlazorApp.Models
{
    [FirestoreData]
    public class Student{
        [FirestoreProperty]
        public string Id {get; set;} = Guid.NewGuid().ToString();
        [FirestoreProperty]
        public string? Name {get; set;}
        [FirestoreProperty]
        public string? Email {get; set;}
        [FirestoreProperty]
        public int Age {get; set;}
        [FirestoreProperty]
        public DateTime? Birthday {get; set;}

    }
}