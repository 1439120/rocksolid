using Google.Cloud.Firestore;
using BlazorApp.Models;

namespace BlazorApp.Services
{
    public class StudentService
    {
        private readonly FirestoreDb _firestoreDb;
        private const string CollectionName = "students";

        public StudentService(FirestoreService firestoreService)
        {
            _firestoreDb = firestoreService.GetFirestoreDb();
        }

        // Create
        public async Task<string> AddTodoAsync(Student student)
        {
            DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(student.Id);
            await docRef.SetAsync(student);
            return docRef.Id;
        }

        // Read - Get single todo
        public async Task<Student?> GetTodoAsync(string id)
        {
            DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(id);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            
            if (snapshot.Exists)
            {
                return snapshot.ConvertTo<Student>();
            }
            return null;
        }

        // Read - Get all todos
        public async Task<List<Student>> GetAllTodosAsync()
        {
            Query query = _firestoreDb.Collection(CollectionName);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            
            return querySnapshot.Documents
                .Select(document => document.ConvertTo<Student>())
                .ToList();
        }

        // Read - Get todos with filtering
        public async Task<List<Student>> GetTodosByUserAsync(string userId)
        {
            Query query = _firestoreDb.Collection(CollectionName)
                .WhereEqualTo("UserId", userId)
                .OrderByDescending("CreatedAt");
                
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            
            return querySnapshot.Documents
                .Select(document => document.ConvertTo<Student>())
                .ToList();
        }

        // Update
        public async Task UpdateTodoAsync(Student student)
        {
            DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(student.Id);
            await docRef.SetAsync(student, SetOptions.MergeAll);
        }

        // Delete
        public async Task DeleteTodoAsync(string id)
        {
            DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(id);
            await docRef.DeleteAsync();
        }

        // Real-time updates
        public void ListenToTodos(Action<QuerySnapshot> onChange)
        {
            Query query = _firestoreDb.Collection(CollectionName);
            FirestoreChangeListener listener = query.Listen(snapshot =>
            {
                onChange(snapshot);
            });
        }
    }
}