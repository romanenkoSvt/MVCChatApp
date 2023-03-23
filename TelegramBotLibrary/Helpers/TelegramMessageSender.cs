using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBotLibrary.Commands;

namespace TelegramBotLibrary.Helpers
{
    public class TelegramMessageSender : ITelegramMessageSender
    {
        private readonly ITelegramBotClient _botClient;

        public TelegramMessageSender(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public async Task<Message> SendMessageAsync(IReplyMarkup keyboard, string message, ChatId chatId)
        {
            Message sentMessage = await _botClient.SendTextMessageAsync(
                   chatId: chatId,
                   text: message,
                   replyMarkup: keyboard,
                   replyToMessageId: null);

            return sentMessage;
        }

        public async Task RemoveInlineKeyboardAsync(ChatId chatId, int lastSendedMesssageId)
        {
            await _botClient.EditMessageReplyMarkupAsync(
                           chatId,
                           lastSendedMesssageId,
                           replyMarkup: null);
        }

        public async Task EditTextMessageAsync(ChatId chatId, int lastSendedMesssageId, string editedMessage)
        {
            await _botClient.EditMessageTextAsync(
                                  chatId,
                                  lastSendedMesssageId,
                                  editedMessage,
                                  replyMarkup: null);
        }
    }
}
