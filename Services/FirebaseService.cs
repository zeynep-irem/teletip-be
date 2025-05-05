
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin.Auth;
using Google.Cloud.Firestore;


namespace Teletipbe.Services
{
    public class FirebaseService
    {
        public FirebaseService()
        {
            // Firebase initialization gerekli ise burada yapılabilir
        }

        // Kullanıcıyı e-posta adresi ile getir
        public async Task<UserRecord> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(email);
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving user by email: {ex.Message}");
            }
        }

        // Yeni kullanıcı oluştur
        public async Task<string> CreateUserAsync(string email, string password, string name)
        {
            try
            {
                var args = new UserRecordArgs
                {
                    Email = email,
                    Password = password,
                    DisplayName = name,
                };

                var user = await FirebaseAuth.DefaultInstance.CreateUserAsync(args);
                return user.Uid;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating user: {ex.Message}");
            }
        }
    }
}
