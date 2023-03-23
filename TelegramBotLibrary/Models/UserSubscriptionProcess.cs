using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotLibrary.Commands;

namespace TelegramBotLibrary.Models
{
    public class UserSubscriptionProcess
    {
        public IAppCommand CurrentCommand { get; set; }
        public List<int> UpdateIds { get; set; } = new List<int>(); //All message ids from user for duplicate check
        public List<IAppCommand> Commands { get; set; } = new List<IAppCommand>();
    }
}
