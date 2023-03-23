using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotLibrary.Enums;

namespace TelegramBotLibrary.Commands
{
    public class SubscriptionEndCommand : BaseAppCommand
    {
        public override (IReplyMarkup keyboard, string message) CreateBotMessage()
        {

            return (null, $"You was successfully {(UserActionTypes)SubscriptionInfo.UserActionType}d.{Environment.NewLine}" +
                 $"{Environment.NewLine}---------------------" +
                $"{Environment.NewLine}Send any message to subscribe or unsubscribe for offline message receiving.");
        }

        public override void FinishCurrentCommand()
        {
        }

        public override IAppCommand GetNewObject()
        {
            return new SubscriptionEndCommand();
        }

        public override IAppCommand GetNextCommand()
        {
            throw new NotImplementedException();
        }

        public override (bool isValid, string errMessage) ValidateResponse()
        {
            throw new NotImplementedException();
        }
    }
}
