using EFCoreLibrary;
using EFCoreLibrary.Models;
using EFCoreLibrary.Repos;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotLibrary.Models;

namespace TelegramBotLibrary.Commands
{
    public abstract class BaseAppCommand : IAppCommand
    {
        private const string CancellationString = "Cancel";
        public SignInManager<ApplicationUser> SignInManager { get; set; }
        public UserManager<ApplicationUser> UserManager { get; set; }
        public ApplicationDbContext Context { get; set; }
        public SubscriptionInfoModel SubscriptionInfo { get; set; } = new SubscriptionInfoModel();
        public Update Update { get; set; }
        public abstract (IReplyMarkup keyboard, string message) CreateBotMessage();
        public abstract (bool isValid, string errMessage) ValidateResponse();

        public abstract void FinishCurrentCommand();
        public abstract IAppCommand GetNextCommand();
        public abstract IAppCommand GetNewObject(); //just for history, do not need filled properties in current command

        protected void CheckForCancellation()
        {
            if (Update.CallbackQuery?.Data == CancellationString || Update.Message?.Text?.Trim().ToLower() == CancellationString.ToLower())
            {
                throw new OperationCanceledException();
            }
        }
    }
}
