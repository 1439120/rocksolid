// using FirebaseAdmin;
// using FirebaseAdmin.Auth;
// using Google.Apis.Auth.OAuth2;

// public class FirebaseAuthService
// {
//     public FirebaseAuthService()
//     {
//         if (FirebaseApp.DefaultInstance == null)
//         {
//             FirebaseApp.Create(new AppOptions()
//             {
//                 Credential = GoogleCredential.GetApplicationDefault(),
//             });
//         }
//     }

//     public async Task<string> VerifyTokenAsync(string idToken)
//     {
//         var decoded = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
//         return decoded.Uid;
//     }
// }