using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bloog.Infrastructure
{
    public interface IBlogGateway
    {
        Task<int> CreateBlogAsync(string name);
        Task<Blog> FindBlogAsync(int id);
        Task SaveChangesAsync(Dictionary<int, Dictionary<string, PropertyChange>> updates);
    }
}
