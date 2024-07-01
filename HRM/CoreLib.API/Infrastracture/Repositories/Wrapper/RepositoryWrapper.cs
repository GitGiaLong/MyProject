using CoreLib.API.Infrastracture.Repositories.DbContext;
using CoreLib.API.Infrastracture.Repositories.Identity;
using CoreLib.API.Infrastracture.Repositories.System;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace CoreLib.API.Infrastracture.Repositories.Wrapper
{

    public class RepositoryWrapper : IRepositoryWrapper
    {
        /*private IClientSessionHandle session { get; }
		public IDisposable Session => session;*/

        private readonly IMongoContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        public RepositoryWrapper(IMongoContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public async Task<bool> CommitAsync()
        {
            int changeAmount = await _context.SaveChanges();
            Dispose();
            return changeAmount > 0;
        }
        public void Dispose()
        {
            //_context.Dispose();
            //GC.SuppressFinalize(this);
        }

        public async Task<bool> CommitAsyncTransaction(IClientSessionHandle session)
        {
            //session.CommitTransaction();
            int changeAmount = await _context.SaveChangesTransaction(session);
            Dispose();
            return changeAmount > 0;
        }
        #region Identity - Đăng Nhập, Phân Quyền, ...
        private ILoginRepository login;
        public ILoginRepository Login => login ??= new LoginRepository(_context, _contextAccessor);
        #endregion

        #region System - Hỗ trợ hệ thống
        private ICounterRepository counter;
        public ICounterRepository Counter => counter ??= new CounterRepository(_context, _contextAccessor);
        #endregion
    }
}
