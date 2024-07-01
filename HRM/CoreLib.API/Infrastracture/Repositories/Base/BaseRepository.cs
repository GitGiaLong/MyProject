using CoreLib.API.Infrastracture.Repositories.DbContext;
using CoreLib.Extensions;
using Entities.Application.Base;
using Entities.Application.Base.Pagings;
using Entities.System.Companes;
using Entities.System.StaticClaim;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Globalization;
using System.Reflection;

namespace CoreLib.API.Infrastracture.Repositories.Base
{

    public abstract class BaseRepository<T> : IBaseRepository<T> where T : IEntityBase
    {
        protected readonly IMongoContext _context;
        protected readonly IHttpContextAccessor _accessor;

        protected IMongoCollection<T> _mongoCollection;
        private readonly FilterDefinition<T> _filterBase;
        private FilterDefinition<T> _filterFinal;


        /// <summary>
        /// Username đăng nhập
        /// </summary>
        protected string CurrentUser => _accessor.HttpContext.User.Identity.Name;

        private string companyId;
        protected string CompanyId
        {
            get
            {
                if (_accessor.HttpContext.User.Claims.Any())
                {
                    companyId = _accessor.HttpContext.User.FindFirst(StaticClaimTypes.CompanyId).Value ?? string.Empty;
                }
                return companyId;
            }
        }

        protected BaseRepository(IMongoContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
            _mongoCollection = _context.GetCollection<T>(typeof(T).Name);
            _filterBase = Builders<T>.Filter.Eq("IsActive", true);
            _filterFinal = FilterDefinition<T>.Empty;

            // nếu Entity có CompanyId thì lọc theo luôn!
            if (typeof(IEntityBaseCompany).IsAssignableFrom(typeof(T)) && !string.IsNullOrEmpty(CompanyId))
            {
                _filterBase &= Builders<T>.Filter.Eq(StaticClaimTypes.CompanyId, new ObjectId(CompanyId));
            }
        }

        public bool CheckExist(FilterDefinition<T> filter = null)
        {
            _filterFinal = _filterBase;
            if (filter != null)
            {
                _filterFinal &= filter;
            }

            return _mongoCollection.Find(_filterFinal).Any();
        }

        public Task<bool> CheckExistAsync(FilterDefinition<T> filter = null)
        {
            _filterFinal = _filterBase;
            if (filter != null)
            {
                _filterFinal &= filter;
            }
            return Task.Run(() => _mongoCollection.Find(_filterFinal).AnyAsync());
        }

        public async Task<long> Count(FilterDefinition<T> filter = null)
        {
            _filterFinal = _filterBase;
            if (filter != null)
            {
                _filterFinal &= filter;
            }
            return await _mongoCollection.CountDocumentsAsync(_filterFinal).ConfigureAwait(false);
        }

        public T FindFirstOne(FilterDefinition<T> filter = null)
        {
            _filterFinal = _filterBase;
            if (filter != null)
            {
                _filterFinal &= filter;
            }

            return _mongoCollection.Find(_filterFinal).FirstOrDefault();
        }

        public Task<T> FindFirstOneAsync(FilterDefinition<T> filter = null)
        {
            _filterFinal = _filterBase;
            if (filter != null)
            {
                _filterFinal &= filter;
            }

            return Task.Run(() => _mongoCollection.Find(_filterFinal).FirstOrDefaultAsync());
        }

        public Task<T> FindLastOneAsync(FilterDefinition<T> filter = null)
        {
            _filterFinal = _filterBase;
            if (filter is not null)
            {
                _filterFinal &= filter;
            }

            SortDefinition<T> sort = Builders<T>.Sort.Descending("Id");
            return Task.Run(() => _mongoCollection.Find(_filterFinal).Sort(sort).FirstOrDefaultAsync());
        }

        public T FindLastOne(FilterDefinition<T> filter = null)
        {
            _filterFinal = _filterBase;
            if (filter is not null)
            {
                _filterFinal &= filter;
            }

            SortDefinition<T> sort = Builders<T>.Sort.Descending("Id");
            return _mongoCollection.Find(_filterFinal).Sort(sort).FirstOrDefault();
        }

        public List<T> GetAll(FilterDefinition<T> filter = null, SortDefinition<T> sort = null)
        {
            _filterFinal = _filterBase;
            if (filter != null)
            {
                _filterFinal &= filter;
            }
            return sort != null ? _mongoCollection.Find(_filterFinal).Sort(sort).ToList() : _mongoCollection.Find(_filterFinal).ToList();
        }

        public Task<List<T>> GetAllAsync(FilterDefinition<T> filter = null, SortDefinition<T> sort = null)
        {
            _filterFinal = _filterBase;

            if (filter != null)
            {
                _filterFinal &= filter;
            }
            return sort != null
                ? Task.Run(() => _mongoCollection.Find(_filterFinal).Sort(sort).ToListAsync())
                : Task.Run(() => _mongoCollection.Find(_filterFinal).ToListAsync());
        }

        public T GetById(string id, FilterDefinition<T> filter = null)
        {
            _filterFinal = _filterBase;

            _filterFinal &= Builders<T>.Filter.Eq("_id", new ObjectId(id));

            if (filter != null)
            {
                _filterFinal &= filter;
            }

            return _mongoCollection.Find(_filterFinal).FirstOrDefault();
        }

        public Task<T> GetByIdAsync(string id, FilterDefinition<T> filter = null)
        {
            _filterFinal = _filterBase;

            _filterFinal &= Builders<T>.Filter.Eq("_id", new ObjectId(id));

            if (filter != null)
            {
                _filterFinal &= filter;
            }
            return Task.Run(() => _mongoCollection.Find(_filterFinal).FirstOrDefaultAsync());
        }

        public DataTableResponse<T> GetPaging(DataTablePaging objPg, FilterDefinition<T> filter = null)
        {
            if (filter != null)
            {
                _filterFinal &= filter;
            }

            if (objPg == null)
            {
                return new DataTableResponse<T>();
            }

            // objPg.pageSize = objPg.length != null ? Convert.ToInt32(objPg.length) : 0;

            //bản chất, Convert.ToInt32 sẽ trả về 0 khi giá trị cần convert là null nên khỏi dùng câu lệnh ktra ở trên
            objPg.PageSize = Convert.ToInt32(objPg.Length, CultureInfo.CurrentCulture);
            //mặc định sort tăng dần theo Code
            SortDefinition<T> v_Sort = Builders<T>.Sort.Ascending("Code");
            objPg.SortColumn = char.ToUpper(objPg.SortColumn[0], CultureInfo.CurrentCulture) + objPg.SortColumn[1..];
            //nếu có chỉ định column cần sort và chỉ thị sort (asc, desc) thì làm theo
            if (objPg.SortColumnDirection != null && objPg.SortColumnDirection.Equals("desc", StringComparison.OrdinalIgnoreCase))
            {
                v_Sort = Builders<T>.Sort.Descending(objPg.SortColumn ?? "Code");
            }

            //lấy tổng số documents có trong collection 
            //ước lượng dựa vào metadata nên nó sẽ chạy nhanh hơn (lượng data lớn), nhưng nếu có nhiều cluster thì có thể không chính xác 
            long totalDocuments = _mongoCollection.EstimatedDocumentCount();

            List<T> res;
            //lấy tất cả những document đang active

            if (!string.IsNullOrEmpty(objPg.SearchValue))
            {
                objPg.SearchValue = "/.*" + objPg.SearchValue + ".*/";
                try
                {
                    //trùng tên sẽ tự động không tạo nữa, nhưng khác tên mà trùng key ($**) thì báo lỗi exception
                    //_collection.Indexes.CreateOne(Builders<T>.IndexKeys.Text("$**"), new CreateIndexOptions() { DefaultLanguage = "english", Name = "TextIndex" });
                    // Phương pháp tạo index mới thay thế cho dòng code ở trên đã bị Obsolete
                    IndexKeysDefinition<T> keys = Builders<T>.IndexKeys.Text("$**");
                    CreateIndexOptions indexOptions = new() { DefaultLanguage = "english", Name = "TextIndex" };
                    CreateIndexModel<T> model = new(keys, indexOptions);
                    _mongoCollection.Indexes.CreateOne(model);
                }
                catch (Exception) { }

                //nếu có điều kiện tìm kiếm (searchValue có giá trị) thì lọc theo điều kiện trước khi lấy
                FilterDefinition<T> v_search = Builders<T>.Filter.Text(objPg.SearchValue, new TextSearchOptions { CaseSensitive = false });
                _filterFinal &= v_search;
            }

            //tìm kiếm xem có filter theo column hay không, nếu có thì add thêm filter vào
            if (objPg.SearchArray != null && objPg.SearchArray.Count > 0)
            {
                foreach (KeyValuePair<string, string> pair in objPg.SearchArray)
                {
                    _filterFinal &= new BsonDocument { { pair.Key, new BsonDocument { { "$regex", pair.Value }, { "$options", "i" } } } };
                }
            }

            //lấy tổng số documents sau khi đã lọc theo điều kiện tìm kiếm
            long recordsFiltered = _mongoCollection.CountDocuments(_filterFinal);

            res = objPg.PageSize == -1
                ? _mongoCollection.Find(_filterFinal)
                 .Sort(v_Sort)
                 .ToList()
                : _mongoCollection.Find(_filterFinal)
                     .Skip(Convert.ToInt32(objPg.Start))
                     .Limit(objPg.PageSize)
                     .Sort(v_Sort)
                     .ToList();

            if (recordsFiltered == 0)
            {
                recordsFiltered = totalDocuments;
            }

            //trả về dữ liệu theo chuẩn paging của datatable.net
            DataTableResponse<T> resjs = new(objPg.Draw, totalDocuments, recordsFiltered, res);
            return resjs;
        }

        public List<T> SearchMatchArray(string docPropertyName, List<string> lstValue, FilterDefinition<T> filter = null, SortDefinition<T> sort = null)
        {
            _filterFinal = _filterBase;
            //reset
            _filterFinal &= Builders<T>.Filter.In(docPropertyName, lstValue.Distinct());

            if (filter != null)
            {
                _filterFinal &= filter;
            }

            return sort != null ? _mongoCollection.Find(_filterFinal).Sort(sort).ToList() : _mongoCollection.Find(_filterFinal).ToList();
        }

        public Task<List<T>> SearchMatchArrayAsync(string docPropertyName, List<string> lstValue, FilterDefinition<T> filter = null, SortDefinition<T> sort = null)
        {
            _filterFinal = _filterBase;
            //reset
            _filterFinal &= Builders<T>.Filter.In(docPropertyName, lstValue.Distinct());

            if (filter != null)
            {
                _filterFinal &= filter;
            }

            return sort != null
                ? Task.Run(() => _mongoCollection.Find(_filterFinal).Sort(sort).ToListAsync())
                : Task.Run(() => _mongoCollection.Find(_filterFinal).ToListAsync());
        }

        public void Update(string id, T t, string[] arrExceptField = null, IClientSessionHandle session = null)
        {
            UpdateDefinitionBuilder<T> builder = Builders<T>.Update;
            UpdateDefinition<T> update = null;
            // Set value cho các trường mặc định
            t.Id = null;
            t.IsActive = true;
            if (typeof(IEntityBase).IsAssignableFrom(typeof(T)))
            {
                t.SetUpdater(CurrentUser, DateTime.Now);
            }
            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                CustomAttributeData ignore = prop.CustomAttributes.ToList().Find(x => x.AttributeType.Name is "BsonIgnoreAttribute" or "BsonIgnoreIfNullAttribute");
                if (ignore != null)
                {
                    continue;
                }
                // Mongo không cho phép thay đổi Mongo IDs
                if (prop.PropertyType == typeof(ObjectId))
                {
                    continue;
                }
                // Những trường nào thuộc arrExceptField thì không update
                if (arrExceptField != null && arrExceptField.Contains(prop.Name))
                {
                    continue;
                }
                // Nếu không có giá trị nào được gán thì giữ nguyên cái đang có dưới database
                if (prop.GetValue(t) == null)
                {
                    continue;
                }
                // Nếu là thuộc tính datetime thì C# tự gán ngày mặc định chứ không phải null, phải kiểm tra và bỏ qua nó
                if (prop.PropertyType == typeof(DateTime))
                {
                    if (Convert.ToDateTime(prop.GetValue(t)) == DateTime.MinValue)
                    {
                        continue;
                    }
                }
                // Gán lần đầu tiên, cũng chính là gán Object Id
                update = update == null ? builder.Set(prop.Name, prop.GetValue(t)) : update.Set(prop.Name, prop.GetValue(t));
            }

            BsonDocument filter = new("_id", new ObjectId(id));

            UpdateOptions option = new()
            {
                IsUpsert = false
            };
            if (session == null)
                _context.AddCommand(async () => await _mongoCollection.UpdateOneAsync(filter, update, option).ConfigureAwait(false));
            else
                _context.AddCommand(async () => await _mongoCollection.UpdateOneAsync(session, filter, update, option).ConfigureAwait(false));
        }

        public void UpdateMany(IList<T> list, IClientSessionHandle session = null)
        {
            List<WriteModel<T>> updates = new();
            FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;

            foreach (T document in list)
            {
                foreach (PropertyInfo prop in typeof(T).GetProperties())
                {
                    if (prop.Name == "Id")
                    {
                        if (prop.GetValue(document) != null)
                        {
                            FilterDefinition<T> filter = filterBuilder.Eq(prop.Name, prop.GetValue(document));
                            if (typeof(IEntityBase).IsAssignableFrom(typeof(T)))
                            {
                                document.SetUpdater(CurrentUser, DateTime.Now);
                            }
                            updates.Add(new ReplaceOneModel<T>(filter, document));
                            break;
                        }
                    }
                }
            }
            if (session == null)
                _context.AddCommand(async () => await _mongoCollection.BulkWriteAsync(updates));
            else
                _context.AddCommand(async () => await _mongoCollection.BulkWriteAsync(session, updates));
        }

        public async Task<bool> UpdateManyWithoutCommandTask(IList<T> list, IClientSessionHandle session = null)
        {
            List<WriteModel<T>> updates = new();
            FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;

            foreach (T document in list)
            {
                foreach (PropertyInfo prop in typeof(T).GetProperties())
                {
                    if (prop.Name == "Id")
                    {
                        if (prop.GetValue(document) != null)
                        {
                            FilterDefinition<T> filter = filterBuilder.Eq(prop.Name, prop.GetValue(document));
                            if (typeof(IEntityBase).IsAssignableFrom(typeof(T)))
                            {
                                document.SetUpdater(CurrentUser, DateTime.Now);
                            }
                            updates.Add(new ReplaceOneModel<T>(filter, document));
                            break;
                        }
                    }
                }
            }
            BulkWriteResult result = session == null
                ? await _mongoCollection.BulkWriteAsync(updates).ConfigureAwait(false)
                : (BulkWriteResult)await _mongoCollection.BulkWriteAsync(session, updates).ConfigureAwait(false);
            return result.IsModifiedCountAvailable;
        }

        public void UpdateCustomizeField(FilterDefinition<T> filter, BsonDocument bsUpdate, IClientSessionHandle session = null)
        {
            _filterFinal = _filterBase;
            _filterFinal &= filter;

            BsonDocument bs = new(bsUpdate)
            {
                { "UpdatedOn", DateTime.Now },
                { "UpdatedBy", CurrentUser }
            };
            UpdateDefinition<T> update = new BsonDocument("$set", bs);
            if (session == null)
                _context.AddCommand(async () => await _mongoCollection.UpdateManyAsync(filter, update).ConfigureAwait(false));
            else
                _context.AddCommand(async () => await _mongoCollection.UpdateManyAsync(session, filter, update).ConfigureAwait(false));
        }

        public void Delete(string id, IClientSessionHandle session = null)
        {
            //check null
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException($"{typeof(T).Name}: Base.Delete - id is null");
            }

            FilterDefinition<T> filter = new BsonDocument("_id", new ObjectId(id));
            UpdateDefinition<T> update = new BsonDocument("$set", new BsonDocument {
                { "IsActive", false }, { "UpdatedOn", DateTime.Now }, { "UpdatedBy", CurrentUser }
            });
            if (session == null)
                _context.AddCommand(async () => await _mongoCollection.UpdateOneAsync(filter, update));
            else
                _context.AddCommand(async () => await _mongoCollection.UpdateOneAsync(session, filter, update));
        }

        public void Insert(T t, IClientSessionHandle session = null)
        {
            if (typeof(IEntityBase).IsAssignableFrom(typeof(T)))
            {
                //((IEntityBase)t).CreatedOn = DateTime.Now;
                t.SetCreator(CurrentUser, DateTime.Now);
            }
            t.Id = null;

            if (session == null)
                _context.AddCommand(async () => await _mongoCollection.InsertOneAsync(t));
            else
                _context.AddCommand(async () => await _mongoCollection.InsertOneAsync(session, t));
        }

        public void InsertMany(IList<T> list, IClientSessionHandle session = null)
        {
            InsertManyOptions options = new() { IsOrdered = true };
            foreach (T t in list)
            {
                if (typeof(IEntityBase).IsAssignableFrom(typeof(T)))
                {
                    t.SetCreator(CurrentUser, DateTime.Now);
                }
                t.Id = null;
            }
            if (session == null)
                _context.AddCommand(async () => await _mongoCollection.InsertManyAsync(list, options));
            else
                _context.AddCommand(async () => await _mongoCollection.InsertManyAsync(session, list, options));
        }

        public async Task<string> InsertWithoutCommandTask(T t, IClientSessionHandle session = null)
        {
            if (typeof(IEntityBase).IsAssignableFrom(typeof(T)))
            {
                t.SetCreator(CurrentUser, DateTime.Now);
            }
            t.Id = null;
            if (session == null)
                await _mongoCollection.InsertOneAsync(t);
            else
                await _mongoCollection.InsertOneAsync(session, t);

            return t.Id;
        }

        public async Task InsertManyWithoutCommandTask(IList<T> list, IClientSessionHandle session = null)
        {
            InsertManyOptions options = new() { IsOrdered = true };
            foreach (T t in list)
            {
                if (typeof(IEntityBase).IsAssignableFrom(typeof(T)))
                {
                    t.SetCreator(CurrentUser, DateTime.Now);
                }
                t.Id = null;
            }
            if (session == null)
                await _mongoCollection.InsertManyAsync(list, options);
            else
                await _mongoCollection.InsertManyAsync(session, list, options);
        }

        public async Task<CursorResponse<List<T>>> PagingCursorSuSu(CursorFilter paginationFilter, List<BsonDocument> bsonElements = null, List<BsonElement> match = null, List<BsonDocument> option = null, string viewName = null)
        {
            try { if (!string.IsNullOrEmpty(viewName)) _mongoCollection = _context.GetCollection<T>(viewName); }
            catch (Exception) { throw new Exception(viewName + " không tồn tại"); }

            if (paginationFilter.PageSize < 1)
            {
                paginationFilter.PageSize = 20;
            }

            // xài cách này, phải biết điểm cursor bắt đầu..
            // do mặc định sắp giảm _id ( mới nhất luôn hiện đầu )
            bool firstCursor = false;
            if (string.IsNullOrEmpty(paginationFilter.Cursor))
            {
                T tmpT = await _mongoCollection.Find(_filterBase).Sort(Builders<T>.Sort.Descending("Id")).FirstOrDefaultAsync();
                paginationFilter.Cursor = tmpT.Id;
                firstCursor = true;
            }

            List<BsonDocument> pipeline = new();

            // order by _id desc default!
            #region Order By
            List<BsonElement> OrdeyByIdDesc = new() { new BsonElement("_id", -1) };

            paginationFilter.SortColumn = !string.IsNullOrEmpty(paginationFilter.SortColumn) && !paginationFilter.SortColumn.Equals("id", StringComparison.OrdinalIgnoreCase)
                ? char.ToUpper(paginationFilter.SortColumn[0], CultureInfo.CurrentCulture) + paginationFilter.SortColumn[1..]
                : "_id";

            if (string.IsNullOrEmpty(paginationFilter.SortColumnDirection))
            {
                if (paginationFilter.SortColumn != "_id")
                    OrdeyByIdDesc.Add(new BsonElement(paginationFilter.SortColumn, -1));
            }
            else //nếu có chỉ định column cần sort và chỉ thị sort (asc, desc) thì làm theo
            {
                if (paginationFilter.SortColumn != "_id")
                {
                    if (paginationFilter.SortColumnDirection.Equals("desc", StringComparison.OrdinalIgnoreCase))
                        OrdeyByIdDesc.Add(new BsonElement(paginationFilter.SortColumn, -1));
                    else
                        OrdeyByIdDesc.Add(new BsonElement(paginationFilter.SortColumn, 1));
                }
            }
            pipeline.Add(new BsonDocument("$sort", new BsonDocument(OrdeyByIdDesc)));
            #endregion

            #region Where
            BsonDocument matchPage = new()
            {
                new BsonElement("IsActive", new BsonBoolean(true)),
                new BsonElement("_id", new BsonDocument(firstCursor is true ? "$lte" : "$lt", new ObjectId(paginationFilter.Cursor)))
            };
            if (typeof(IEntityBaseCompany).IsAssignableFrom(typeof(T)))
            {
                matchPage.Add(new BsonElement("CompanyId", new ObjectId(CompanyId)));
            }
            // Nếu cần bổ sung thêm điều kiện match !
            if (match is not null)
            {
                matchPage.AddRange(match);
            }

            if (string.IsNullOrEmpty(paginationFilter.Text) && (string.IsNullOrEmpty(paginationFilter.Columns) || paginationFilter.Columns.RemoveSpaces().Equals("{}")))
            {
                #region default get all
                pipeline.Add(new BsonDocument("$match", matchPage));
                #endregion
            }
            else
            {
                #region Columns
                if (!string.IsNullOrEmpty(paginationFilter.Columns))
                {
                    // Filter each column
                    Dictionary<string, dynamic> arrColumns = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(paginationFilter.Columns);

                    if (arrColumns != null && arrColumns.Any())
                    {
                        BsonArray bsonArray = new();
                        foreach (KeyValuePair<string, dynamic> pair in arrColumns)
                        {

                            string key = char.ToUpper(pair.Key[0], CultureInfo.CurrentCulture) + pair.Key[1..];
                            // Nếu là string , xét trường hợp là ngày nữa !!
                            if (pair.Value.GetType().Name == "String")
                            {
                                if (DateTime.TryParse(pair.Value, out DateTime Temp) == true)
                                {
                                    DateTime dateFrom = UDateTime.GetBeginningOfTheDay(Temp);
                                    DateTime dateTo = UDateTime.GetEndingOfTheDay(Temp);
                                    // so sánh theo kiểu ngày với mongo
                                    bsonArray.Add(
                                            new BsonDocument(key, new BsonDocument
                                            {
                                                { "$gte", new BsonDateTime(dateFrom) },
                                                { "$lte", new BsonDateTime(dateTo) }
                                            })
                                    );
                                }
                                else
                                {
                                    string query = $".*{pair.Value}.*";
                                    bsonArray.Add(new BsonDocument().Add(key, new BsonDocument()
                                                                            .Add("$regex", query)
                                                                            .Add("$options", "i")));
                                }

                            }
                            // Nếu là số !
                            else if (pair.Value.GetType().Name == "Int64")
                            {
                                bsonArray.Add(new BsonDocument(key, pair.Value));
                            }
                        }
                        if (match is null) // Ko có filter theo company và vendor !
                        {
                            matchPage.Add("$and", bsonArray);
                        }
                        else  // Có filter thì phải add thêm bsonarray vào option, nếu ko sẽ lỗi duplicated $and
                        {
                            // bsonArray sau khi xử lý theo column, add thêm option vào
                            bsonArray.AddRange(option);
                            // reset lại
                            matchPage = new()
                            {
                                { "IsActive", new BsonBoolean(true) },
                                { "$and", bsonArray }
                            };
                        }
                        pipeline.Add(new BsonDocument("$match", matchPage));
                    }

                }
                #endregion

                #region Text
                else if (!string.IsNullOrEmpty(paginationFilter.Text))
                {
                    // create Index first
                    //await CreateIndexText(paginationFilter.Text);
                    string text = "/.*" + paginationFilter.Text + ".*/";
                    text = text.Replace("-", "\\-");
                    matchPage.Add("$text", new BsonDocument("$search", text.Trim()));
                    pipeline.Add(new BsonDocument("$match", matchPage));
                }
                #endregion
            }
            #endregion

            // take pageSize!
            pipeline.Add(new BsonDocument("$limit", paginationFilter.PageSize));


            // add pipe join collection
            if (bsonElements is not null)
            {
                pipeline.AddRange(bsonElements);
            }
            List<T> pagedData = await _mongoCollection.Aggregate<T>(pipeline).ToListAsync().ConfigureAwait(false);
            string cursor = pagedData[^1].Id;

            return new CursorResponse<List<T>>(cursor, pagedData, paginationFilter.PageSize);
        }

        public async Task<PagingResponse<List<T>>> PagingSuSu(
            PagingFilter paginationFilter,
            List<BsonDocument> bsonElements = null,
            List<BsonElement> match = null,
            List<BsonDocument> option = null,
            string viewName = null
        )
        {
            try { if (!string.IsNullOrEmpty(viewName)) _mongoCollection = _context.GetCollection<T>(viewName); }
            catch (Exception) { throw new Exception(viewName + " không tồn tại"); }

            if (paginationFilter.PageNumber < 1)
            {
                paginationFilter.PageNumber = 1;
            }

            if (paginationFilter.PageSize < 1)
            {
                paginationFilter.PageSize = 20;
            }

            List<BsonDocument> pipeline = new();

            GetPipelines(ref pipeline, paginationFilter, bsonElements, match, option);

            List<T> pagedData = await _mongoCollection.Aggregate<T>(pipeline).ToListAsync().ConfigureAwait(false);
            //_mongoCollection.AsQueryable<T>(aggregateOptions: pipeline);

            long totalRecords = pagedData.Count;
            List<T> temp = pagedData.Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                                .Take(paginationFilter.PageSize)
                                .ToList();

            return CreatePagedReponse(temp, paginationFilter, totalRecords);
        }

        private List<BsonDocument> GetPipelines(ref List<BsonDocument> pipeline, PagingFilter paginationFilter,
            List<BsonDocument> bsonElements = null,
            List<BsonElement> match = null,
            List<BsonDocument> option = null)
        {
            BsonDocument matchPage = new("IsActive", new BsonBoolean(true));
            if (typeof(IEntityBaseCompany).IsAssignableFrom(typeof(T)))
            {
                matchPage.Add(new BsonElement("CompanyId", new ObjectId(CompanyId)));
            }
            // Nếu cần bổ sung thêm điều kiện match !
            if (match is not null)
            {
                matchPage.AddRange(match);
            }

            if (string.IsNullOrEmpty(paginationFilter.Text) && (string.IsNullOrEmpty(paginationFilter.Columns) || paginationFilter.Columns.RemoveSpaces().Equals("{}")))
            {
                #region default get all
                pipeline.Add(new BsonDocument("$match", matchPage));
                #endregion
            }
            else
            {
                #region Columns
                if (!string.IsNullOrEmpty(paginationFilter.Columns))
                {
                    // Filter each column
                    Dictionary<string, dynamic> arrColumns = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(paginationFilter.Columns);

                    if (arrColumns != null && arrColumns.Any())
                    {
                        BsonArray bsonArray = new();
                        foreach (KeyValuePair<string, dynamic> pair in arrColumns)
                        {

                            string key = char.ToUpper(pair.Key[0], CultureInfo.CurrentCulture) + pair.Key[1..];
                            // Nếu là string , xét trường hợp là ngày nữa !!
                            if (pair.Value.GetType().Name == "String")
                            {
                                if (DateTime.TryParse(pair.Value, out DateTime Temp) == true)
                                {
                                    DateTime dateFrom = UDateTime.GetBeginningOfTheDay(Temp);
                                    DateTime dateTo = UDateTime.GetEndingOfTheDay(Temp);
                                    // so sánh theo kiểu ngày với mongo
                                    bsonArray.Add(
                                            new BsonDocument(key, new BsonDocument
                                            {
                                                { "$gte", new BsonDateTime(dateFrom) },
                                                { "$lte", new BsonDateTime(dateTo) }
                                            })
                                    );
                                }
                                else
                                {
                                    string query = $".*{pair.Value}.*";
                                    bsonArray.Add(new BsonDocument().Add(key, new BsonDocument()
                                                                            .Add("$regex", query)
                                                                            .Add("$options", "i")));
                                }

                            }
                            // Nếu là số !
                            else if (pair.Value.GetType().Name == "Int64")
                            {
                                bsonArray.Add(new BsonDocument(key, pair.Value));
                            }
                        }
                        if (match is null) // Ko có filter theo company và vendor !
                        {
                            matchPage.Add("$and", bsonArray);
                        }
                        else  // Có filter thì phải add thêm bsonarray vào option, nếu ko sẽ lỗi duplicated $and
                        {
                            // bsonArray sau khi xử lý theo column, add thêm option vào
                            if (option != null)
                                bsonArray.AddRange(option);
                            // reset lại
                            matchPage = new()
                            {
                                { "IsActive", new BsonBoolean(true) },
                                { "$and", bsonArray }
                            };
                            if (typeof(IEntityBaseCompany).IsAssignableFrom(typeof(T)))
                            {
                                matchPage.Add(new BsonElement("CompanyId", new ObjectId(CompanyId)));
                            }
                            if (option == null)
                                matchPage.AddRange(match);
                        }
                        pipeline.Add(new BsonDocument("$match", matchPage));
                    }

                }
                #endregion

                #region Text
                else if (!string.IsNullOrEmpty(paginationFilter.Text))
                {
                    // create Index first
                    //await CreateIndexText(paginationFilter.Text);
                    string text = "/.*" + paginationFilter.Text + ".*/";
                    text = text.Replace("-", "\\-");
                    matchPage.Add("$text", new BsonDocument("$search", text.Trim()));
                    pipeline.Add(new BsonDocument("$match", matchPage));
                }
                #endregion
            }

            paginationFilter.SortColumn = !string.IsNullOrEmpty(paginationFilter.SortColumn) && !paginationFilter.SortColumn.Equals("id", StringComparison.OrdinalIgnoreCase)
                ? char.ToUpper(paginationFilter.SortColumn[0], CultureInfo.CurrentCulture) + paginationFilter.SortColumn[1..]
                : "_id";

            if (string.IsNullOrEmpty(paginationFilter.SortColumnDirection))
            {
                pipeline.Add(new BsonDocument("$sort", new BsonDocument(paginationFilter.SortColumn, -1)));
            }
            else //nếu có chỉ định column cần sort và chỉ thị sort (asc, desc) thì làm theo
            {
                if (paginationFilter.SortColumnDirection.Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    pipeline.Add(new BsonDocument("$sort", new BsonDocument(paginationFilter.SortColumn, -1)));
                }
                else
                {
                    pipeline.Add(new BsonDocument("$sort", new BsonDocument(paginationFilter.SortColumn, 1)));
                }
            }

            // add pipe join collection
            if (bsonElements is not null)
            {
                pipeline.AddRange(bsonElements);
            }

            return pipeline;
        }

        private static PagingResponse<List<T>> CreatePagedReponse(List<T> pagedData, PagingFilter validFilter, long totalRecords)
        {
            PagingResponse<List<T>> respose = new(pagedData, validFilter.PageNumber, validFilter.PageSize);
            double totalPages = totalRecords / (double)validFilter.PageSize;
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            respose.TotalPages = roundedTotalPages;
            respose.TotalRecords = totalRecords;
            return respose;
        }

        public async Task<List<BsonDocument>> Aggregate(List<BsonDocument> pipelines, string viewName = null)
        {
            try { if (!string.IsNullOrEmpty(viewName)) _mongoCollection = _context.GetCollection<T>(viewName); }
            catch (Exception) { throw new Exception(viewName + " không tồn tại"); }
            return await _mongoCollection.Aggregate<BsonDocument>(pipelines).ToListAsync().ConfigureAwait(false);
        }

        void IDisposable.Dispose()
        {
            //_context?.Dispose();
            //GC.SuppressFinalize(this);
        }

        public async Task<PagingResponse<List<BsonDocument>>> PagingSusuAggregate(PagingFilter paginationFilter, List<BsonDocument> bsonElements = null, List<BsonElement> match = null, List<BsonDocument> option = null, string viewName = null)
        {
            try { if (!string.IsNullOrEmpty(viewName)) _mongoCollection = _context.GetCollection<T>(viewName); }
            catch (Exception) { throw new Exception(viewName + " không tồn tại"); }

            if (paginationFilter.PageNumber < 1)
            {
                paginationFilter.PageNumber = 1;
            }

            if (paginationFilter.PageSize < 1)
            {
                paginationFilter.PageSize = 20;
            }

            List<BsonDocument> pipeline = new();

            GetPipelines(ref pipeline, paginationFilter, bsonElements, match, option);

            List<BsonDocument> pagedData = await Aggregate(pipeline, viewName);

            long totalRecords = pagedData.Count;
            List<BsonDocument> temp = pagedData.Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                                .Take(paginationFilter.PageSize)
                                .ToList();

            PagingResponse<List<BsonDocument>> response = new(temp, paginationFilter.PageNumber, paginationFilter.PageSize);
            double totalPages = totalRecords / (double)paginationFilter.PageSize;
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            response.TotalPages = roundedTotalPages;
            response.TotalRecords = totalRecords;
            return response;
        }
    }
}
