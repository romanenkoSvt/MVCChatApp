using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramBotLibrary.Commands
{
    public interface ICommandManager
    {
        long ChatId { get; set; }
        Update Update { get; set; }

        Task Invoke();
    }
}
