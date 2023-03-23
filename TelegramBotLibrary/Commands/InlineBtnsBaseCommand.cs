using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using TelegramBotLibrary.Models;

namespace TelegramBotLibrary.Commands
{
    public abstract class InlineBtnsBaseCommand : BaseAppCommand, IInlineBtnsCommand
    {
        public List<ButtonsDataModel> Buttons { get; set; }

        public override (bool isValid, string errMessage) ValidateResponse()
        {
            CheckForCancellation();            

            if (Update.Type != UpdateType.CallbackQuery)
            {
                return (false, $"Please, choose answer by pressing appropriate button.{Environment.NewLine}");
            }

            string errMessage = "";
            
            string value = Update.CallbackQuery?.Data;          
            bool isInt = int.TryParse(value, out int answerId);

            bool isValid = isInt && Buttons.Any(ct => ct.Id == answerId);
            errMessage = !isValid ? $"Please, choose answer by pressing appropriate button.{Environment.NewLine}" : errMessage;

            return (isValid, errMessage);
        }

        public abstract string EditMessageWithInlineKeyboardMarkup();
    }
}