namespace MVCChatApp.Areas.Admin.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ChatName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsBanned { get; set; }
        public string UserRole { get; set; }
        public string Status { get; set; }
    }
}
