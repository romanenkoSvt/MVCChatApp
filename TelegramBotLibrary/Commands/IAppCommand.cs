using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using TelegramBotLibrary.Models;
using System.Windows.Input;
using EFCoreLibrary.Repos;
using EFCoreLibrary;
using EFCoreLibrary.Models;
using Microsoft.AspNetCore.Identity;

namespace TelegramBotLibrary.Commands
{
    public interface IAppCommand
    {
        public SignInManager<ApplicationUser> SignInManager { get; set; }
        public ApplicationDbContext Context { get; set; }
        SubscriptionInfoModel SubscriptionInfo { get; set; }
        UserManager<ApplicationUser> UserManager {get; set;}
        Update Update { get; set; }
        (IReplyMarkup keyboard, string message) CreateBotMessage();
        (bool isValid, string errMessage) ValidateResponse();
        void FinishCurrentCommand();
        IAppCommand GetNextCommand();
        IAppCommand GetNewObject();
    }
}
