using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreLibrary.Models
{
    public class OfflineSubscriptionUser
    {
        public int Id { get; set; }        
        public long TelegramChatId { get; set; }
        public DateTime UpdateDate { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
