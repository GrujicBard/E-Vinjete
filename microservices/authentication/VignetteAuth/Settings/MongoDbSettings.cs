namespace VignetteAuth.Settings
{
    public class MongoDbSettings: IMongoDbSettings
    {
        public string UsersCollectionName { get; set; }
        public string CarsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IMongoDbSettings
    {
        string UsersCollectionName { get; set; }
        string CarsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
