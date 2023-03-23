using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCChatApp.Areas.Admin.Models;
using EFCoreLibrary;
using EFCoreLibrary.Models;

namespace MVCChatApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(UsersFilteredModel vModel)
        {
            vModel ??= new UsersFilteredModel();

            vModel.Users = from u in _context.Users
                           join ur in _context.UserRoles
                            on u.Id equals ur.UserId into uur
                           from res1 in uur.DefaultIfEmpty()

                           join r in _context.Roles
                            on res1.RoleId equals r.Id into uurr
                           from res2 in uurr.DefaultIfEmpty()
                           select new UserModel() 
                           {
                               Id = u.Id,   
                               ChatName = u.ChatName,
                               Email = u.Email,
                               FirstName = u.FirstName,
                               IsBanned = u.IsBanned,
                               LastName = u.LastName,
                               PhoneNumber = u.PhoneNumber,
                               UserRole = res2.Name,
                               Status = u.IsBanned ? "Заблокирован" : "Активен"
                           };

            vModel.FilterUsers();

            ViewBag.IsBanned = GetIsBannedFilter();

            return View(vModel);
        }

        public IActionResult BlockUser(string id, bool isBanned)
        {
            ApplicationUser user = _context.Users.SingleOrDefault(u => u.Id == id);
            user.IsBanned = isBanned;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        private SelectList GetIsBannedFilter()
        {
            return new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "Заблокированные", Value = true.ToString()},
                new SelectListItem {Text = "Активные", Value = false.ToString()},
            }, "Value", "Text");
        }
    }
}
