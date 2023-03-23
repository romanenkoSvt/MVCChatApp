using EFCoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramBotLibrary.Enums;

namespace TelegramBotLibrary.Models
{
    public class SubscriptionInfoModel
    {
        public int UserActionType { get; set; }
        public User MessageSender { get; set; }
        public int? LastSendedMesssageId { get; set; }
        public bool HasInlineKeyboard { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int NumberOfAttempts { get; set; } = 0;
    }
}
