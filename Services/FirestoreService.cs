using Google.Cloud.Firestore;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace BlazorApp.Services
{
    public class FirestoreService
    {
        public FirestoreDb FirestoreDb { get; } // Public property
        public FirestoreDb GetFirestoreDb() => FirestoreDb;

        public FirestoreService(IConfiguration configuration)
        {
            var projectId = configuration["FIREBASE_PROJECT_ID"] 
                           ?? configuration["Firebase:ProjectId"] 
                           ?? throw new InvalidOperationException("Firebase ProjectId is required");

            // Find the service account file
            var credentialPath = FindServiceAccountFile();
            
            // Create FirestoreDb using the builder pattern
            var builder = new FirestoreDbBuilder
            {
                ProjectId = projectId,
                Credential = GoogleCredential.FromFile(credentialPath)
            };
            
            FirestoreDb = builder.Build();
        }

        private string FindServiceAccountFile()
        {
            // Check multiple possible locations
            var possiblePaths = new[]
            {
                Path.Combine(Directory.GetCurrentDirectory(), "firebase-service-account.json"),
                Path.Combine(Directory.GetCurrentDirectory(), "Credentials", "firebase-service-account.json"),
                Path.Combine(AppContext.BaseDirectory, "firebase-service-account.json"),
                Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS")
            };

            foreach (var path in possiblePaths)
            {
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    return path;
                }
            }

            throw new FileNotFoundException(
                "Firebase service account file not found. Please download it from Firebase Console " +
                "and save it as 'firebase-service-account.json' in your project root.");
        }
        
    }

}