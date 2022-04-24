using MongoDB.Driver;
using VignetteAuth.Models;
using VignetteAuth.Settings;

namespace VignetteAuth.DataAccess
{
    public class UserDataAccess
    {
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Car> _cars;
        public UserDataAccess(IMongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<User>(settings.UsersCollectionName);
            _cars = database.GetCollection<Car>(settings.CarsCollectionName);
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _users.Find(s => true).ToListAsync();
        }
        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _users.Find<User>(s => s.Id == id).FirstOrDefaultAsync();
        }
        public async Task<User> GetByIdWithCarsAsync(string id)
        {
            var user = new User();
            user = await GetUserByIdAsync(id);
            if(user != null)
            {
                if (user.Cars != null && user.Cars.Count > 0)
                {
                    user.CarsList = await _cars.Find<Car>(c => user.Cars.Contains(c.Id)).ToListAsync();
                }
            }
            return user;
        }
        public async Task<User> CreateUserAsync(User user)
        {
            await _users.InsertOneAsync(user);
            return user;
        }
        public async Task UpdateUserAsync(string id, User user)
        {
            await _users.ReplaceOneAsync(s => s.Id == id, user);
        }
        public async Task DeleteUserAsync(string id)
        {
            await _users.DeleteOneAsync(s => s.Id == id);
        }

        public async Task<Car> CreateCarAsync(Car car)
        {
            await _cars.InsertOneAsync(car);
            return car;
        }
        public async Task UpdateCarAsync(string id, Car car)
        {
            await _cars.ReplaceOneAsync(s => s.Id == id, car);
        }
        public async Task DeleteCarAsync(string id)
        {
            await _cars.DeleteOneAsync(s => s.Id == id);
        }

    }
}
