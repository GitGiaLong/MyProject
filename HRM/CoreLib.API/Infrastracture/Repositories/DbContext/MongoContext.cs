
namespace CoreLib.API.Infrastracture.Repositories.DbContext
{

    public class MongoContext 
    {
        private readonly List<Func<Task>> _commands;

        public MongoContext()
        {
            // Tất cả các câu lệnh(thêm, sửa, delete,...) sẽ được lưu vào đây, sẽ được thực thi lúc SaveChanges
            _commands = new List<Func<Task>>();
        }

        public void AddCommand(Func<Task> func)
        {
            _commands.Add(func);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<int> SaveChanges()
        {
            IEnumerable<Task> commandTasks = _commands.Select(c => c());
            await Task.WhenAll(commandTasks);
            Dispose();
            return _commands.Count;
        }

        public async Task<int> SaveChangesTransaction()
        {
            IEnumerable<Task> commandTasks = _commands.Select(c => c());
            await Task.WhenAll(commandTasks);

            Dispose();
            return _commands.Count;
        }

        private void ConfigureMongo()
        {
            //MongoClient ??= new MongoClient(MongoSetting.Connection);
            //Database = MongoClient.GetDatabase(MongoSetting.DatabaseName);
        }
    }
}
