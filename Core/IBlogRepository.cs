using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bloog
{
    public interface IBlogRepository : IDisposable
    {
        void AddAsync(Blog blog);
        Task<Blog> FindAsync(Guid id);
        Task<IEnumerable<Blog>> FindByUserAsync(string username);
    }
}
