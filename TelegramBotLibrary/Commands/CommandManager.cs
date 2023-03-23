using EFCoreLibrary;
using EFCoreLibrary.Models;
using EFCoreLibrary.Repos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotLibrary.Helpers;
using TelegramBotLibrary.Models;

namespace TelegramBotLibrary.Commands
{
    public class CommandManager : ICommandManager
    {
        private static Dictionary<long, UserSubscriptionProcess> _clientRequests = new();
        private readonly IConfiguration _config;
        private readonly ITelegramMessageSender _telegramMessageSender;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserSubscriptionProcess UserSubscription { get; set; }

        public CommandManager(IConfiguration config, ITelegramMessageSender telegramMessageSender,
            ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _config = config;
            _telegramMessageSender = telegramMessageSender;
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public long ChatId { get; set; }
        public Update Update { get; set; }

        public async Task Invoke()
        {
            try
            {
                bool isSubscribtionProcessRunning = _clientRequests.TryGetValue(ChatId, out UserSubscriptionProcess userRegistration);

                if (!isSubscribtionProcessRunning) //If it is first message from user
                {
                    await RegistrationStart();
                }
                else
                {
                    UserSubscription = userRegistration;

                    if (UserSubscription.UpdateIds.Any(x => x == Update.Id)) // duplicate check
                    {
                        return;
                    }

                    UserSubscription.UpdateIds.Add(Update.Id);
                    IAppCommand command = UserSubscription.CurrentCommand;
                    InitCommand(command);

                    IReplyMarkup keyboard;
                    string message;

                    (bool isValid, string errMessage) = command.ValidateResponse();

                    if (!isValid)
                    {
                        await RemoveInlineKeyboardAsync();

                        ++UserSubscription.CurrentCommand.SubscriptionInfo.NumberOfAttempts;

                        if(UserSubscription.CurrentCommand.SubscriptionInfo.NumberOfAttempts > 5)
                        {
                            throw new InvalidOperationException("Too many wrong attempts. Subscription process terminated");
                        }

                        (keyboard, message) = command.CreateBotMessage();

                        if (!string.IsNullOrEmpty(errMessage))
                        {
                            errMessage += $"---------------------{Environment.NewLine}";
                        }

                        message = $"{errMessage}{message}";
                        await SendMessageAsync(keyboard, message);
                    }
                    else
                    {
                        command.FinishCurrentCommand();
                        await EditTextMessageAsync(command);
                        UserSubscription.Commands.Add(command.GetNewObject());

                        IAppCommand nextCommand = command.GetNextCommand();
                        nextCommand.SubscriptionInfo = UserSubscription.CurrentCommand.SubscriptionInfo;
                        nextCommand.Update = Update;
                        (keyboard, message) = nextCommand.CreateBotMessage();
                        UserSubscription.CurrentCommand = nextCommand;

                        await SendMessageAsync(keyboard, message);

                        if (nextCommand is SubscriptionEndCommand)
                        {
                            _clientRequests.Remove(ChatId);
                        }
                    }
                }
            }
            catch (ApiRequestException e) when (e.Message.Contains("Bad Request: message is not modified"))
            {
                // do nothing
            }
            catch (OperationCanceledException)
            {
                await Clean($"You cancelled process.{Environment.NewLine}---------------------{Environment.NewLine}Send any message to start again.");
            }
            catch (InvalidOperationException ioe)
            { 
                await Clean(ioe.Message);                
            }
            catch (Exception ex)
            {
                await Clean("Subscription process terminated. Unexpected error. Try again later.");
                throw;
            }
        }

        private async Task RegistrationStart()
        {
            UserSubscription = new()
            {
                CurrentCommand = new GetLoginAndPassCommand(),                
            };

            InitCommand(UserSubscription.CurrentCommand);
            UserSubscription.UpdateIds.Add(Update.Id);

            _clientRequests.Add(ChatId, UserSubscription);

            (IReplyMarkup keyboard, string message) = UserSubscription.CurrentCommand.CreateBotMessage();
            await SendMessageAsync(keyboard, message);
        }

        private async Task EditTextMessageAsync(IAppCommand command)
        {
            int lastSendedMessage = (int)UserSubscription.CurrentCommand.SubscriptionInfo.LastSendedMesssageId;

            if (command is IInlineBtnsCommand inlineBtnsCommmand
                && UserSubscription.CurrentCommand.SubscriptionInfo.HasInlineKeyboard) //if there has been Inlinekeybord, remove buttons from chat
            {
                string editedMessage = inlineBtnsCommmand.EditMessageWithInlineKeyboardMarkup();
                await _telegramMessageSender.EditTextMessageAsync(ChatId, lastSendedMessage, editedMessage);

                _clientRequests[ChatId].CurrentCommand.SubscriptionInfo.HasInlineKeyboard = false;               
            }
        }

        private async Task RemoveInlineKeyboardAsync()
        {
            if (UserSubscription.CurrentCommand.SubscriptionInfo.HasInlineKeyboard)
            {
                _clientRequests[ChatId].CurrentCommand.SubscriptionInfo.HasInlineKeyboard = false;

                await _telegramMessageSender.RemoveInlineKeyboardAsync(ChatId, (int)UserSubscription.CurrentCommand.SubscriptionInfo.LastSendedMesssageId);
            }
        }

        private async Task<Message> SendMessageAsync(IReplyMarkup keyboard, string message)
        {
            Message sentMessage = await _telegramMessageSender.SendMessageAsync(keyboard, message, ChatId);

            if (UserSubscription?.CurrentCommand.SubscriptionInfo is not null)
            {
                UserSubscription.CurrentCommand.SubscriptionInfo.LastSendedMesssageId = sentMessage.MessageId;
            }
            
            return sentMessage;
        }

        private void InitCommand(IAppCommand command)
        {
            command.Update = Update;
            command.UserManager = _userManager;
            command.SignInManager = _signInManager;
            command.Context = _context;
        }

        private async Task Clean(string message)
        {
            await RemoveInlineKeyboardAsync();
            await SendMessageAsync(null, message);
            _clientRequests.Remove(ChatId);
        }
    }
}