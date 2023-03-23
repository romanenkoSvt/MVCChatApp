using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using TelegramBotLibrary.Commands;
using TelegramBotLibrary.Helpers;

namespace TelegramApi.Controllers
{
  //  [Route("api/[controller]")]
    public class WebhookController : Controller
    {
        private ICommandManager _commandManager;

        public WebhookController(ICommandManager commandManager)
        {
            _commandManager = commandManager;
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Update update)
        {
            long? chatId = update?.Message?.Chat.Id ?? update?.CallbackQuery?.From.Id ?? update?.EditedMessage.From.Id;

            try
            {
                _commandManager.ChatId = (long)chatId;
                _commandManager.Update = update;
                await _commandManager.Invoke();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            return Ok(); //If telegram do not receives Ok as reply it will continue to send the same message
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok("Hi");
        }
    }
}
