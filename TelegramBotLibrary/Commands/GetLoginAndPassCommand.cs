using EFCoreLibrary;
using EFCoreLibrary.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotLibrary.Enums;

namespace TelegramBotLibrary.Commands
{
    public class GetLoginAndPassCommand : BaseAppCommand
    {      
        public override (IReplyMarkup keyboard, string message) CreateBotMessage()
        {
            return (null, $"This Bot can subscribe or unsubscribe you for offline message receiving. Please, follow instructions. To cancel process press 'Cancel' button or just write \"Cancel\"" +
                $"{Environment.NewLine}---------------------" +
                $"{Environment.NewLine}Please send your login and password separated by whitespace." +
                $"{Environment.NewLine}---------------------" +
                $"{Environment.NewLine}Example: ex@example.com passHere");
        }

        public override void FinishCurrentCommand()
        {
            OfflineSubscriptionUser offlineSubscriptionUser = Context.OfflineSubscriptionUser.SingleOrDefault(u => u.ApplicationUserId == SubscriptionInfo.ApplicationUser.Id);

            if (offlineSubscriptionUser == null)
            {
                SubscriptionInfo.UserActionType = (int)UserActionTypes.Subscribe;
            }
            else
            {
                SubscriptionInfo.UserActionType = (int)UserActionTypes.Unsubscribe;
            }
        }

        public override IAppCommand GetNewObject()
        {
           return new GetLoginAndPassCommand();
        }

        public override IAppCommand GetNextCommand()
        {
            return new UserActionTypeCommand();
        }

        public override (bool isValid, string errMessage) ValidateResponse()
        {
            CheckForCancellation();

            string errMessage = $"Please, provide correct login ang password{Environment.NewLine}";

            if (string.IsNullOrEmpty(Update.Message?.Text?.Trim()))
                return (false, errMessage);

            char separator = ' ';
            string[] loginPassword = Update.Message?.Text.Split(separator);

            if(loginPassword.Length != 2)
                return (false, errMessage);

            var user = UserManager.FindByEmailAsync(loginPassword[0])?.Result;

            if (user == null)
                return (false, errMessage);

            bool res = SignInManager.UserManager.CheckPasswordAsync(user, loginPassword[1]).Result;

            if (res)
            {
                SubscriptionInfo.ApplicationUser = user;
            }

            return (res, res ? "" : errMessage);
        }
    }
}
