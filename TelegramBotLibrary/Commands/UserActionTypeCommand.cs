using EFCoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotLibrary.Enums;
using TelegramBotLibrary.Helpers;
using TelegramBotLibrary.Models;

namespace TelegramBotLibrary.Commands
{
    public class UserActionTypeCommand : InlineBtnsBaseCommand
    {
        public override (IReplyMarkup keyboard, string message) CreateBotMessage()
        {
            UserActionTypes action = (UserActionTypes)SubscriptionInfo.UserActionType;
            Buttons = null;
            Buttons = new List<ButtonsDataModel> { new ButtonsDataModel { Id = (int)action, Name = action.ToString() } };

            InlineKeyboardMarkup keyboard = KeyboardCreator.GetKeyboard(Buttons);
            string message = $"Hello, {SubscriptionInfo.ApplicationUser.ChatName}" +
                $"{Environment.NewLine}---------------------" +
                $"{Environment.NewLine}Please confirm the action by clicking the appropriate button:";

            SubscriptionInfo.HasInlineKeyboard = true;

            return (keyboard, message);
        }

        public override string EditMessageWithInlineKeyboardMarkup()
        {
            return $"Your application is being processed. Please wait.{Environment.NewLine}";
        }

        public override void FinishCurrentCommand()
        {
            if (SubscriptionInfo.UserActionType == (int)UserActionTypes.Subscribe)
            {
                OfflineSubscriptionUser offlineSubscriptionUser = new()
                {
                    ApplicationUserId = SubscriptionInfo.ApplicationUser.Id,
                    TelegramChatId = Update.CallbackQuery.From.Id,
                    UpdateDate = DateTime.Now,
                };

                Context.OfflineSubscriptionUser.Add(offlineSubscriptionUser);
                Context.SaveChanges();
            }

            if (SubscriptionInfo.UserActionType == (int)UserActionTypes.Unsubscribe)
            {
                OfflineSubscriptionUser offlineSubscriptionUser = Context.OfflineSubscriptionUser
                    .SingleOrDefault(u => u.ApplicationUserId == SubscriptionInfo.ApplicationUser.Id);               

                Context.OfflineSubscriptionUser.Remove(offlineSubscriptionUser);
                Context.SaveChanges();
            }
        }

        public override IAppCommand GetNewObject()
        {
            return new UserActionTypeCommand();
        }

        public override IAppCommand GetNextCommand()
        {
            return new SubscriptionEndCommand();
        }
    }
}
