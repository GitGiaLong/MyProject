using CoreLib.API.Infrastracture.Repositories.Base;
using CoreLib.API.Infrastracture.Repositories.DbContext;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace CoreLib.API.Infrastracture.Repositories.System
{
    public class CounterRepository : BaseRepository<Counter>, ICounterRepository
    {
        public CounterRepository(IMongoContext context, IHttpContextAccessor accessor) : base(context, accessor)
        {
        }

        /// <summary>
        /// Kiểm tra và update index + 1
        /// </summary>
        /// <param name="code"></param>
        /// <returns>Trả về 0 nếu không tìm thấy code trong hệ thống, index+ 1</returns>
        public async Task<int> NextAutoIndex(string code, IClientSessionHandle session = null)
        {
            var obj = await FindFirstOneAsync(Builders<Counter>.Filter.Where(p => p.Code == code));
            if (obj != null)
            {
                obj.AutoIndex += 1;
                if (session == null)
                    Update(obj.Id, obj);
                else
                    Update(obj.Id, obj, session: session);
            }
            else
            {
                obj = new Counter(code, 1);
                if (session == null)
                    Insert(obj);
                else
                    Insert(obj, session);
            }
            return obj.AutoIndex;
        }
    }
}
