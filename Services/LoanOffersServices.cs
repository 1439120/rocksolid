using Google.Cloud.Firestore;
using BlazorApp.Models;

namespace BlazorApp.Services
{
    public class LoanOffersServices
    {
        private const string CollectionName = "loan_offers";
        private readonly FirestoreDb _firestoreDb;

        public LoanOffersServices(FirestoreService firestoreService)
        {
            _firestoreDb = firestoreService.FirestoreDb;
        }

        // Create
        public async Task<string> Add(LoanOffer loanOffer)
        {
             // Generate ID if not provided
            if (string.IsNullOrEmpty(loanOffer.Id))
            {
                loanOffer.Id = Guid.NewGuid().ToString();
            }

            DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(loanOffer.Id);
            await docRef.SetAsync(loanOffer);
            return docRef.Id;
        }
        // Read
        public async Task<List<LoanOffer>> Find(string loanOfferId)
        {
            Query query = _firestoreDb.Collection(CollectionName)
                .WhereEqualTo("Id", loanOfferId);
                
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            
            return querySnapshot.Documents
                .Select(document => document.ConvertTo<LoanOffer>())
                .Where(offer => offer != null)
                .ToList()!;
        }

        // Read - Get all students
        public async Task<List<LoanOffer>> GetAllLoanOffersAsync()
        {
            Query query = _firestoreDb.Collection(CollectionName);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            
            return querySnapshot.Documents
                .Select(document => document.ConvertTo<LoanOffer>())
                .Where(offer => offer != null)
                .ToList()!;
        }


        // Update
        public async Task UpdateLoanOffer(LoanOffer loanOffer)
        {
            if (string.IsNullOrEmpty(loanOffer.Id))
            {
                throw new ArgumentException("Loan offer ID cannot be null or empty");
            }

            DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(loanOffer.Id);
            await docRef.SetAsync(loanOffer, SetOptions.MergeAll);
        }

        // Update partial fields
        public async Task UpdateLoanOfferFieldsAsync(string loanOfferId, Dictionary<string, object> updates)
        {
            DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(loanOfferId);
            await docRef.UpdateAsync(updates);
        }

        // Delete
        public async Task DeleteAsync(string id)
        {
            DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(id);
            await docRef.DeleteAsync();
        }

        // Real-time updates for loan offers
        public FirestoreChangeListener ListenToLoanOffers(Action<QuerySnapshot> onChange)
        {
            Query query = _firestoreDb.Collection(CollectionName);
            return query.Listen(snapshot =>
            {
                onChange(snapshot);
            });
        }

    }
}