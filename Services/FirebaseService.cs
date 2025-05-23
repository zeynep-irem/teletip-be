// Services/FirebaseService.cs
using FirebaseAdmin.Auth;

namespace Teletipbe.Services
{
    public class FirebaseService
    {
        public FirebaseService()
        {
            // FirebaseApp.InitializeApp(...) 
            // FirebaseAdmin konfigurasyonu zaten Program.cs tarafında yapıldıysa gerek yok
        }

        public Task<UserRecord> GetUserByUidAsync(string uid)
            => FirebaseAuth.DefaultInstance.GetUserAsync(uid);

        public Task<UserRecord> GetUserByEmailAsync(string email)
            => FirebaseAuth.DefaultInstance.GetUserByEmailAsync(email);

        public Task<string> CreateUserAsync(string email, string password)
        {
            var args = new UserRecordArgs
            {
                Email = email,
                Password = password
            };
            return FirebaseAuth.DefaultInstance
                       .CreateUserAsync(args)
                       .ContinueWith(t => t.Result.Uid);
        }

        public Task DeleteUserAsync(string uid)
            => FirebaseAuth.DefaultInstance.DeleteUserAsync(uid);
    }
}
