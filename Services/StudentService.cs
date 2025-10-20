using Google.Cloud.Firestore;
using BlazorApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace BlazorApp.Services
{
    public class StudentService
    {
        // private readonly FirestoreDb _firestoreDb;
        // private static readonly string CollectionName = "students";
        private const string CollectionName = "students";
        private readonly FirestoreDb _firestoreDb;

        public StudentService(FirestoreService firestoreService)
        {
            _firestoreDb = firestoreService.FirestoreDb;
        }

        // Create
        public async Task<string> AddStudentAsync(Student student)
        {
            // Generate ID if not provided
            if (string.IsNullOrEmpty(student.Id))
            {
                student.Id = Guid.NewGuid().ToString();
            }

            DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(student.Id);
            await docRef.SetAsync(student);
            return docRef.Id;
        }

        // Read - Get single student
        public async Task<Student?> GetStudentAsync(string id)
        {
            DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(id);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            
            if (snapshot.Exists)
            {
                return snapshot.ConvertTo<Student>();
            }
            return null;
        }

        // Read - Get all students
        public async Task<List<Student>> GetAllStudentsAsync()
        {
            Query query = _firestoreDb.Collection(CollectionName);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            
            return querySnapshot.Documents
                .Select(document => document.ConvertTo<Student>())
                .Where(student => student != null)
                .ToList()!;
        }

        // Read - Get students with filtering by course
        public async Task<List<Student>> GetStudentsByCourseAsync(string course)
        {
            Query query = _firestoreDb.Collection(CollectionName)
                .WhereEqualTo("Course", course)
                .OrderBy("LastName");
                
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            
            return querySnapshot.Documents
                .Select(document => document.ConvertTo<Student>())
                .Where(student => student != null)
                .ToList()!;
        }

        // Read - Get students with filtering by year
        public async Task<List<Student>> GetStudentsByYearAsync(int year)
        {
            Query query = _firestoreDb.Collection(CollectionName)
                .WhereEqualTo("Year", year)
                .OrderBy("LastName");
                
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            
            return querySnapshot.Documents
                .Select(document => document.ConvertTo<Student>())
                .Where(student => student != null)
                .ToList()!;
        }

        // Update
        public async Task UpdateStudentAsync(Student student)
        {
            if (string.IsNullOrEmpty(student.Id))
            {
                throw new ArgumentException("Student ID cannot be null or empty");
            }

            DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(student.Id);
            await docRef.SetAsync(student, SetOptions.MergeAll);
        }

        // Update partial fields
        public async Task UpdateStudentFieldsAsync(string studentId, Dictionary<string, object> updates)
        {
            DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(studentId);
            await docRef.UpdateAsync(updates);
        }

        // Delete
        public async Task DeleteStudentAsync(string id)
        {
            DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(id);
            await docRef.DeleteAsync();
        }

        // Check if student exists
        public async Task<bool> StudentExistsAsync(string id)
        {
            DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(id);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            return snapshot.Exists;
        }

        // Get students with pagination
        public async Task<(List<Student> Students, string? LastDocumentId)> GetStudentsPagedAsync(int pageSize, string? lastDocumentId = null)
        {
            Query query = _firestoreDb.Collection(CollectionName)
                .OrderBy("LastName")
                .Limit(pageSize);

            if (!string.IsNullOrEmpty(lastDocumentId))
            {
                var lastDoc = _firestoreDb.Collection(CollectionName).Document(lastDocumentId);
                var lastSnapshot = await lastDoc.GetSnapshotAsync();
                if (lastSnapshot.Exists)
                {
                    query = query.StartAfter(lastSnapshot);
                }
            }

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            var students = querySnapshot.Documents
                .Select(document => document.ConvertTo<Student>())
                .Where(student => student != null)
                .ToList()!;

            string? nextLastId = querySnapshot.Documents.Count > 0 
                ? querySnapshot.Documents.Last().Id 
                : null;

            return (students, nextLastId);
        }

        // Real-time updates for students
        public FirestoreChangeListener ListenToStudents(Action<QuerySnapshot> onChange)
        {
            Query query = _firestoreDb.Collection(CollectionName);
            return query.Listen(snapshot =>
            {
                onChange(snapshot);
            });
        }

        // Search students by name
        public async Task<List<Student>> SearchStudentsByNameAsync(string searchTerm)
        {
            // Note: Firestore doesn't support full-text search natively
            // This is a basic implementation that checks contains on client side
            var allStudents = await GetAllStudentsAsync();
            
            return allStudents
                .Where(s => 
                    (s.FirstName?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true) ||
                    (s.LastName?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true) ||
                    (s.Email?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true))
                .ToList();
        }
    }
}