using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EFCoreLibrary.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string ChatName { get; set; }

        [Required]
        public bool IsBanned { get; set; }

        public OfflineSubscriptionUser OfflineSubscriptionUser { get; set; }
    }
}
