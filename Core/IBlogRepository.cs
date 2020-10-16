using System;
using System.Threading.Tasks;

namespace Bloog
{
    public interface IBlogRepository : IDisposable
    {
        void AddAsync(Blog blog);
        Task<Blog> FindAsync(Guid id);
    }
}
