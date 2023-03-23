using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using EFCoreLibrary.Models;
using MVCChatApp.Models;
using System.Diagnostics;

namespace MVCChatApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly HubConnection _hubConnection;

        private readonly string _chatHubUrl;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;

            _chatHubUrl = _config.GetValue<string>("ChatHubUrl");

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_chatHubUrl)
                .WithAutomaticReconnect()
                .Build();           
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser currUser = await _userManager.GetUserAsync(User);
            ViewBag.UserChatName = currUser.ChatName;

            await Send(currUser.ChatName);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task Send(string chatName)
        {
            if(_hubConnection is not null)
            {                
                await _hubConnection.StartAsync();
                await _hubConnection.SendAsync("SendMessage", string.Empty, $"Пользователь {chatName} присоединился к чату");
            }
        }
    }
}