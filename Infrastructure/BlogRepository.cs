using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bloog.Infrastructure
{
    public class BlogRepository : IBlogRepository
    {
        private readonly IBlogGateway gateway;
        private readonly Dictionary<Guid, Dictionary<string, PropertyChange>> Updates = new Dictionary<Guid, Dictionary<string, PropertyChange>>();

        public BlogRepository(IBlogGateway gateway)
        {
            this.gateway = gateway ?? throw new ArgumentNullException(nameof(gateway));
        }

        public async void AddAsync(Blog blog)
        {
            int newId = await gateway.CreateBlogAsync(blog.Name);

            blog.GetType().GetProperty("Id").SetValue(blog, newId, null);
        }

        public async Task<Blog> FindAsync(Guid id)
        {
            var blog = await gateway.FindBlogAsync(id);

            blog.OnNameChanged += HandleBlogNameChanged;

            return blog;
        }

        private void HandleBlogNameChanged(object sender, PropertyChangedEventArgs<Guid, string> e)
        {
            if (!Updates.ContainsKey(e.Key))
                Updates[e.Key] = new Dictionary<string, PropertyChange>();

            Updates[e.Key]["Name"] = new PropertyChange(e.OldValue, e.NewValue);
        }

        public void DiscardChanges()
        {
            Updates.Clear();
        }

        public async void SaveChangesAsync()
        {
            await gateway.SaveChangesAsync(Updates);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    DiscardChanges();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BlogRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
