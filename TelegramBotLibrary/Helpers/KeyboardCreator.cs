using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotLibrary.Models;

namespace TelegramBotLibrary.Helpers
{
    internal class KeyboardCreator
    {
        private const string CancellationString = "Cancel";

        public static InlineKeyboardMarkup GetKeyboard(List<ButtonsDataModel> data, int buttonsNumInLine = 2)
        {
            int linesQty = (data.Count + buttonsNumInLine - 1) / buttonsNumInLine; // number of button rows

            // num of buttons rows (+1 for row with cancel button)
            InlineKeyboardButton[][] keyboardButtons = new InlineKeyboardButton[linesQty + 1][];
            InlineKeyboardButton[] keyboardButtonsLine = new InlineKeyboardButton[buttonsNumInLine]; // the button row itself
            int currButtonNum = 0;
            int line = 0;

            for (int i = 0; i < data.Count; ++i)
            {
                ButtonsDataModel keyValue = data.ElementAt(i);

                if (currButtonNum >= buttonsNumInLine)
                {
                    keyboardButtonsLine = new InlineKeyboardButton[buttonsNumInLine];
                    currButtonNum = 0;
                }

                keyboardButtonsLine[currButtonNum] = InlineKeyboardButton.WithCallbackData(text: keyValue.Name, callbackData: $"{keyValue.Id}");

                if (currButtonNum == buttonsNumInLine - 1 || i == data.Count - 1)
                {
                    if (i == data.Count - 1) //clean if in last row there are nulls in array
                    {
                        keyboardButtonsLine = keyboardButtonsLine.Where(k => k != null).ToArray();
                    }

                    keyboardButtons[line] = keyboardButtonsLine;
                    ++line;
                }
                ++currButtonNum;
            }           

            //cancel button

            InlineKeyboardButton[] cancellButtonLine = new InlineKeyboardButton[1];
            cancellButtonLine[0] = InlineKeyboardButton.WithCallbackData(text: CancellationString, callbackData: CancellationString);

            keyboardButtons[line] = cancellButtonLine;
            InlineKeyboardMarkup inlineKeyboard = new(keyboardButtons);

            return inlineKeyboard;
        }
    }
}
