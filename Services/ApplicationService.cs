using Google.Cloud.Firestore;
using BlazorApp.Models;

namespace BlazorApp.Services
{
    public class ApplicationService
    {
        private const string CollectionName = "applications";
        private readonly FirestoreDb _firestoreDb;

        public ApplicationService(FirestoreService firestoreService)
        {
            _firestoreDb = firestoreService.FirestoreDb;
        }

        // Create
        public async Task<string> CreateApplication(Applicant applicant)
        {
            // Generate ID if not provided
            if (string.IsNullOrEmpty(applicant.Id))
            {
                applicant.Id = Guid.NewGuid().ToString();
            }

            // // Debug: Check the actual DateTime values and their kinds
            // Console.WriteLine($"CreatedAt Kind: {applicant.CreatedAt.Kind}, Value: {applicant.CreatedAt}");
            // Console.WriteLine($"UpdatedAt Kind: {applicant.UpdatedAt.Kind}, Value: {applicant.UpdatedAt}");

            // Debug ALL DateTime properties in the Applicant class
            var dateTimeProperties = applicant.GetType().GetProperties()
                .Where(p => p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?));

            Console.WriteLine("=== ALL DateTime Properties ===");
            foreach (var prop in dateTimeProperties)
            {
                try
                {
                    if (prop.PropertyType == typeof(DateTime))
                    {
                        var value = (DateTime)prop.GetValue(applicant);
                        Console.WriteLine($"{prop.Name}: Kind={value.Kind}, Value={value}, IsMinValue={value == DateTime.MinValue}");
                    }
                    else if (prop.PropertyType == typeof(DateTime?))
                    {
                        var value = (DateTime?)prop.GetValue(applicant);
                        Console.WriteLine($"{prop.Name}: HasValue={value.HasValue}, Kind={value?.Kind}, Value={value}, IsMinValue={value == DateTime.MinValue}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{prop.Name}: ERROR - {ex.Message}");
                }
            }

            DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(applicant.Id);
            await docRef.SetAsync(applicant);
            return docRef.Id;
        }

        // Read - Get applicant
        public async Task<Applicant?> GetApplicantByID(string id)
        {
            DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(id);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            
            if (snapshot.Exists)
            {
                return snapshot.ConvertTo<Applicant>();
            }
            return null;
        }

        // Read - Get all applications
        public async Task<List<Applicant>> GetAllApplicationsAsync()
        {
            Query query = _firestoreDb.Collection(CollectionName);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            
            return querySnapshot.Documents
                .Select(document => document.ConvertTo<Applicant>())
                .Where(applicant => applicant != null)
                .ToList()!;
        }

        public async Task<List<Applicant>> GetApprovedApplicationsAsync()
        {
            Query query = _firestoreDb.Collection(CollectionName);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            
            return querySnapshot.Documents
                .Select(document => document.ConvertTo<Applicant>())
                .Where(applicant => applicant != null && applicant.Status == "Approved")
                .ToList()!;
        }

        // Update
        public async Task UpdateApplicantAsync(Applicant applicant)
        {
            if (string.IsNullOrEmpty(applicant.Id))
            {
                throw new ArgumentException("Applicant ID cannot be null or empty");
            }

            DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(applicant.Id);
            await docRef.SetAsync(applicant, SetOptions.MergeAll);
        }

        // Update partial fields
        public async Task UpdateApplicantFieldsAsync(string applicantId, Dictionary<string, object> updates)
        {
            DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(applicantId);
            await docRef.UpdateAsync(updates);
        }

        // Delete
        public async Task DeleteApplicantAsync(string id)
        {
            DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(id);
            await docRef.DeleteAsync();
        }

        // Check if applicant exists
        public async Task<bool> ApplicantExistsAsync(string id)
        {
            DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(id);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            return snapshot.Exists;
        }

        // Real-time updates for applications
        public FirestoreChangeListener ListenToApplicants(Action<QuerySnapshot> onChange)
        {
            Query query = _firestoreDb.Collection(CollectionName);
            return query.Listen(snapshot =>
            {
                onChange(snapshot);
            });
        }

        // Search applicant by name
        public async Task<List<Applicant>> SearchApplicationByNameAsync(string searchTerm)
        {
            // Note: Firestore doesn't support full-text search natively
            // This is a basic implementation that checks contains on client side
            var allApplications = await GetAllApplicationsAsync();
            
            return allApplications
                .Where(s => 
                    (s.Name?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true) ||
                    (s.Surname?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true) ||
                    (s.Email?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true))
                .ToList();
        }
    }
}