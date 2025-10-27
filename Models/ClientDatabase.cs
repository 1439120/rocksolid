
using System.ComponentModel.DataAnnotations;
using Google.Cloud.Firestore;
namespace BlazorApp.Models
{
    [FirestoreData]
    public class ClientDatabase{
        [FirestoreProperty]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [FirestoreProperty, Required(ErrorMessage = "User Name is required.")]
        public string Name {get;set;} = string.Empty;
        [FirestoreProperty]
        public string Surname {get;set;} = string.Empty;
        [FirestoreProperty]
        public string Email {get; set;}= string.Empty;
        [FirestoreProperty]
        public string Password {get; set;}= string.Empty;
        [FirestoreProperty]
        public string UserId {get; set;}= string.Empty;
        [FirestoreProperty]
        public int NumberOfApplications { get; set; } = 1;
        [FirestoreProperty]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [FirestoreProperty]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

         public ClientDatabase()
        {
            Id = Guid.NewGuid().ToString();
            Name = string.Empty;
            Surname = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            UserId = string.Empty;
            NumberOfApplications = 1;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}