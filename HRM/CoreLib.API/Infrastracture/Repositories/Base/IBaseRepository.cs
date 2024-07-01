using Entities.Application.Base;
using Entities.Application.Base.Pagings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CoreLib.API.Infrastracture.Repositories.Base
{
    public interface IBaseRepository<T> : IDisposable where T : IEntityBase
    {
        /// <summary>
        /// Lấy danh sách theo điều kiện filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        Task<List<T>> GetAllAsync(FilterDefinition<T> filter = null, SortDefinition<T> sort = null);
        List<T> GetAll(FilterDefinition<T> filter = null, SortDefinition<T> sort = null);

        /// <summary>
        /// Lấy theo Key ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<T> GetByIdAsync(string id, FilterDefinition<T> filter = null);
        T GetById(string id, FilterDefinition<T> filter = null);

        /// <summary>
        /// Tìm phần tử đầu tiên theo điều kiện filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<T> FindFirstOneAsync(FilterDefinition<T> filter = null);
        T FindFirstOne(FilterDefinition<T> filter = null);

        /// <summary>
        /// Tìm phần tử cuối cùng theo điều kiện filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<T> FindLastOneAsync(FilterDefinition<T> filter = null);
        T FindLastOne(FilterDefinition<T> filter = null);

        /// <summary>
        /// Kiểm tra tồn tại theo điều kiện
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<bool> CheckExistAsync(FilterDefinition<T> filter = null);
        bool CheckExist(FilterDefinition<T> filter = null);
        Task<long> Count(FilterDefinition<T> filter = null);

        /// <summary>
        /// Hàm này sẽ không Async bởi vì nó chỉ thực hiện 1 việc đưa hàm Insert vào commandTask, mà không trực tiếp thực thi hàm Insert
        /// </summary>
        /// <param name="t"></param>
        void Insert(T t, IClientSessionHandle session = null);
        void InsertMany(IList<T> list, IClientSessionHandle session = null);
        Task<string> InsertWithoutCommandTask(T t, IClientSessionHandle session = null);
        Task InsertManyWithoutCommandTask(IList<T> list, IClientSessionHandle session = null);

        /// <summary>
        /// Hàm này sẽ không Async bởi vì nó chỉ thực hiện 1 việc đưa hàm Update vào commandTask, mà không trực tiếp thực thi hàm Update
        /// </summary>
        /// <param name="id"></param>
        /// <param name="t"></param>
        /// <param name="arrExceptField">Mảng chưa các trường không muốn update xuống data</param>
        void Update(string id, T t, string[] arrExceptField = null, IClientSessionHandle session = null);
        void UpdateMany(IList<T> list, IClientSessionHandle session = null);
        void UpdateCustomizeField(FilterDefinition<T> filter, BsonDocument bsUpdate, IClientSessionHandle session = null);
        Task<bool> UpdateManyWithoutCommandTask(IList<T> list, IClientSessionHandle session = null);

        /// <summary>
        /// Hàm này sẽ không Async bởi vì nó chỉ thực hiện 1 việc đưa hàm Update vào commandTask, mà không trực tiếp thực thi hàm Insert
        /// </summary>
        /// <param name="id"></param>
        void Delete(string id, IClientSessionHandle session = null);

        /// <summary>
        /// Tìm danh sách khớp với điều kiện 1 array truyền vào
        /// </summary>
        /// <param name="docPropertyName"></param>
        /// <param name="lstValue">Dạng List</param>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        Task<List<T>> SearchMatchArrayAsync(string docPropertyName, List<string> lstValue, FilterDefinition<T> filter = null, SortDefinition<T> sort = null);
        List<T> SearchMatchArray(string docPropertyName, List<string> lstValue, FilterDefinition<T> filter = null, SortDefinition<T> sort = null);

        /// <summary>
        /// Phân trang DataTable.net - có tìm kiếm theo từng column và tìm kiếm chung cho tất cả columns( text search )
        /// </summary>
        /// <param name="objPg"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        DataTableResponse<T> GetPaging(DataTablePaging objPg, FilterDefinition<T> filter = null);

        /// <summary>
        /// Phân trang customize
        /// </summary>
        /// <param name="paginationFilter"></param>
        /// <param name="bsonElements"></param>
        /// <param name="match"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<PagingResponse<List<T>>> PagingSuSu(PagingFilter paginationFilter,
            List<BsonDocument> bsonElements = null,
            List<BsonElement> match = null,
            List<BsonDocument> option = null,
            string viewName = null);


        Task<CursorResponse<List<T>>> PagingCursorSuSu(CursorFilter paginationFilter,
            List<BsonDocument> bsonElements = null,
            List<BsonElement> match = null,
            List<BsonDocument> option = null,
            string viewName = null);

        Task<List<BsonDocument>> Aggregate(List<BsonDocument> pipelines, string viewName = null);

        Task<PagingResponse<List<BsonDocument>>> PagingSusuAggregate(PagingFilter paginationFilter,
            List<BsonDocument> bsonElements = null,
            List<BsonElement> match = null,
            List<BsonDocument> option = null,
            string viewName = null);

    }
}
