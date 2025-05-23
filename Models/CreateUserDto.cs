namespace Teletipbe.Models
{
    public class CreateUserDto
    {
        public string Name { get; set; }         // Firestore profili için
        public string Email { get; set; }        // Auth ve Firestore
        public string Password { get; set; }     // Sadece Auth
        public string Role { get; set; }         // “Patient” veya “Doctor”
        public string PhoneNumber { get; set; }  // Firestore profili için
    }
}
