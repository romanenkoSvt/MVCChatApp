using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotLibrary.Models;

namespace TelegramBotLibrary.Commands
{
    public interface IInlineBtnsCommand
    {
        public List<ButtonsDataModel> Buttons { get; set; }

        string EditMessageWithInlineKeyboardMarkup();
    }
}
