using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bloog.Infrastructure
{
    public interface IBlogGateway
    {
        Task<int> CreateBlogAsync(Guid id, string name);
        Task<Blog> FindBlogAsync(Guid id);
        Task SaveChangesAsync(Dictionary<Guid, Dictionary<string, PropertyChange>> updates);
        Task<IEnumerable<Blog>> FindByUser(string username);
    }
}
