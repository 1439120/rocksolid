using Google.Cloud.Firestore;
using BlazorApp.Models;

namespace BlazorApp.Services
{
    public class LoanOfferTagsServices
    {
        private const string CollectionName = "loan_offers_tags";
        private readonly FirestoreDb _firestoreDb;

        public LoanOfferTagsServices(FirestoreService firestoreService)
        {
            _firestoreDb = firestoreService.FirestoreDb;
        }

        // Create
        public async Task<string> Add(LoanOffersTags loanOffer)
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
        public async Task<List<LoanOffersTags>> GetTags(string loanOfferId)
        {
            Query query = _firestoreDb.Collection(CollectionName)
                .WhereEqualTo("LoanOfferId", loanOfferId);
                
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            
            return querySnapshot.Documents
                .Select(document => document.ConvertTo<LoanOffersTags>())
                .Where(offer => offer != null)
                .ToList()!;
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