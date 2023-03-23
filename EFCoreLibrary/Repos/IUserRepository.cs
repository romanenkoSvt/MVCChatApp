using EFCoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreLibrary.Repos
{
    public interface IUserRepository : IGenericRepository<ApplicationUser>
    {
    }
}
