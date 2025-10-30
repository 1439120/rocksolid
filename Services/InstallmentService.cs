using Google.Cloud.Firestore;
using BlazorApp.Models;

namespace BlazorApp.Services
{
    public class InstallmentService
    {
        private const string CollectionName = "installments";
        private readonly FirestoreDb _firestoreDb;

        public InstallmentService(FirestoreService firestoreService)
        {
            _firestoreDb = firestoreService.FirestoreDb;
        }

        // Create
        public async Task<string> Add(Installment installment)
        {
             // Generate ID if not provided
            if (string.IsNullOrEmpty(installment.Id))
            {
                installment.Id = Guid.NewGuid().ToString();
            }

            DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(installment.Id);
            await docRef.SetAsync(installment);
            return docRef.Id;
        }
        // Read
        public async Task<List<Installment>> Find(string installmentId)
        {
            Query query = _firestoreDb.Collection(CollectionName)
                .WhereEqualTo("Id", installmentId);
                
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            
            return querySnapshot.Documents
                .Select(document => document.ConvertTo<Installment>())
                .Where(user => user != null)
                .ToList()!;
        }

        // Read - Get installments
        public async Task<List<Installment>> GetInstallments(string ApplicantId)
        {
            Query query = _firestoreDb.Collection(CollectionName)
                .WhereEqualTo("ApplicantId", ApplicantId);
                // .OrderBy("Surname");
                
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            
            return querySnapshot.Documents
                .Select(document => document.ConvertTo<Installment>())
                .Where(user => user != null)
                .ToList()!;
        }

        // Update
        public async Task UpdateInstallment(Installment installment)
        {
            if (string.IsNullOrEmpty(installment.Id))
            {
                throw new ArgumentException("User ID cannot be null or empty");
            }

            DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(installment.Id);
            await docRef.SetAsync(installment, SetOptions.MergeAll);
        }

        // Update partial fields
        public async Task UpdateInstallmentFieldsAsync(string installmentId, Dictionary<string, object> updates)
        {
            DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(installmentId);
            await docRef.UpdateAsync(updates);
        }

        // Delete
        public async Task DeleteAsync(string id)
        {
            DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(id);
            await docRef.DeleteAsync();
        }

        // Real-time updates for installments
        public FirestoreChangeListener ListenToInstallments(Action<QuerySnapshot> onChange)
        {
            Query query = _firestoreDb.Collection(CollectionName);
            return query.Listen(snapshot =>
            {
                onChange(snapshot);
            });
        }

    }
}