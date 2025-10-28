using Google.Cloud.Firestore;
using BlazorApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace BlazorApp.Services
{
    public class ClientDatabaseService
    {
        private const string CollectionName = "client_database";
        private readonly FirestoreDb _firestoreDb;

        public ClientDatabaseService(FirestoreService firestoreService)
        {
            _firestoreDb = firestoreService.FirestoreDb;
        }

        // Create
        public async Task<string> AddUserAsync(ClientDatabase user)
        {
            // Generate ID if not provided
            if (string.IsNullOrEmpty(user.Id))
            {
                user.Id = Guid.NewGuid().ToString();
            }
            Console.WriteLine($"Saving user: {System.Text.Json.JsonSerializer.Serialize(user)}");
            DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(user.Id);
            var userData = new
            {
                user.Id,
                user.Name,
                user.Surname,
                user.Email,
                user.UserId,
                user.Role,
                user.NumberOfApplications,
                user.CreatedAt,
                user.UpdatedAt
            };
            await docRef.SetAsync(userData);
            return docRef.Id;
        }

        // Read - Get single User
        public async Task<ClientDatabase?> GetUserAsync(string id)
        {
            DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(id);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            
            if (snapshot.Exists)
            {
                return snapshot.ConvertTo<ClientDatabase>();
            }
            return null;
        }
        public async Task<List<ClientDatabase>> GetClientByUserId(string UserId)
        {
            Query query = _firestoreDb.Collection(CollectionName)
                .WhereEqualTo("UserId", UserId);
                // .OrderBy("Surname");
                
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            
            return querySnapshot.Documents
                .Select(document => document.ConvertTo<ClientDatabase>())
                .Where(user => user != null)
                .ToList()!;
        }

        // Read - Get all Users
        public async Task<List<ClientDatabase>> GetAllUsersAsync()
        {
            Query query = _firestoreDb.Collection(CollectionName);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            
            return querySnapshot.Documents
                .Select(document => document.ConvertTo<ClientDatabase>())
                .Where(user => user != null)
                .ToList()!;
        }


        // Update
        public async Task UpdateUserAsync(ClientDatabase user)
        {
            if (string.IsNullOrEmpty(user.Id))
            {
                throw new ArgumentException("User ID cannot be null or empty");
            }

            DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(user.Id);
            await docRef.SetAsync(user, SetOptions.MergeAll);
        }

        // Update partial fields
        public async Task UpdateUserFieldsAsync(string userId, Dictionary<string, object> updates)
        {
            DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(userId);
            await docRef.UpdateAsync(updates);
        }

        // Delete
        public async Task DeleteUserAsync(string id)
        {
            DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(id);
            await docRef.DeleteAsync();
        }

        // Check if User exists
        public async Task<bool> UserExistsAsync(string id)
        {
            DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(id);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            return snapshot.Exists;
        }


        // Real-time updates for users
        public FirestoreChangeListener ListenToUsers(Action<QuerySnapshot> onChange)
        {
            Query query = _firestoreDb.Collection(CollectionName);
            return query.Listen(snapshot =>
            {
                onChange(snapshot);
            });
        }

        // Search users by name
        public async Task<List<ClientDatabase>> SearchUsersByNameAsync(string searchTerm)
        {
            // Note: Firestore doesn't support full-text search natively
            // This is a basic implementation that checks contains on client side
            var allUsers = await GetAllUsersAsync();
            
            return allUsers
                .Where(s => 
                    (s.Name?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true) ||
                    (s.Surname?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true) ||
                    (s.Email?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true))
                .ToList();
        }
    }
}