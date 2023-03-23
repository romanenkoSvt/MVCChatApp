using System.Linq;

namespace MVCChatApp.Areas.Admin.Models
{
    public class UsersFilteredModel
    {
        public string SearchString { get; set; }
        public bool? IsBanned { get; set; }
        public IQueryable<UserModel> Users { get; set; }

        public void FilterUsers()
        {
            SearchString = SearchString?.Trim();
            if (!string.IsNullOrEmpty(SearchString))
            {
                Users = Users.Where(u => u.FirstName.ToLower() == SearchString.ToLower()
                    || u.LastName.ToLower() == SearchString.ToLower()
                    || u.ChatName.ToLower() == SearchString.ToLower()
                    || u.Email.ToLower() == SearchString.ToLower());
            }

            if (IsBanned is not null)
            {
                Users = Users.Where(u => u.IsBanned == IsBanned);
            }

            Users = Users.OrderByDescending(u => u.UserRole);
        }
    }
}
