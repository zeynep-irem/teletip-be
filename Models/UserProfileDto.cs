namespace Teletipbe.Models
{
    public class UserProfileDto
    {
        public string Id { get; set; }           // UID / Document ID
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string PhoneNumber { get; set; }
        public string DisplayName { get; set; }  // Auth’daki DisplayName
    }
}