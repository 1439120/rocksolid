using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;

namespace BlazorApp.Services
{
    public class FirestoreService
    {
        private readonly FirestoreDb _firestoreDb;

        public FirestoreService(IConfiguration configuration)
        {
            var projectId = configuration["Firebase:ProjectId"];
            
            // For development - using default credentials
            _firestoreDb = FirestoreDb.Create(projectId);
            
            // For production, you might want to use service account credentials
            // var credentials = GoogleCredential.FromFile("path/to/service-account-key.json");
            // _firestoreDb = FirestoreDb.Create(projectId, credentials);
        }

        public FirestoreDb GetFirestoreDb() => _firestoreDb;
    }
}