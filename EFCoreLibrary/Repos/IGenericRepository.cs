using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreLibrary.Repos
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
      //  Task<T> GetByIdAsync(int id);
        Task<T> GetByIdAsync(string id);
    }
}
