using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotLibrary.Helpers
{
    public interface ITelegramMessageSender
    {
        Task<Message> SendMessageAsync(IReplyMarkup keyboard, string message, ChatId chatId);
        Task RemoveInlineKeyboardAsync(ChatId chatId, int lastSendedMesssageId);
        Task EditTextMessageAsync(ChatId chatId, int lastSendedMesssageId, string editedMessage);
    }
}