using AutoMapper;
using Grpc.Core;
using VignetteAuth.DataAccess;
using VignetteAuth.Protos;
using System.Diagnostics;
using VignetteAuth.Logging;

namespace VignetteAuth.Controllers
{
    public class UserGrpcController : UserService.UserServiceBase
    {
        private readonly UserDataAccess _users;
        private readonly IMapper _mapper;

        public UserGrpcController(UserDataAccess users, IMapper mapper)
        {
            _users = users;
            _mapper = mapper;
        }

        public override async Task<ReturnUsers> GetUsers(VoidRequest request, ServerCallContext context)
        {
            var log = new Log(LogType.INFO, "GET users");
            try
            {
                var users = await _users.GetAllUsersAsync();
                var result = new ReturnUsers
                {
                    Log = log.ToString()
                };

                foreach (var user in users)
                {
                    result.Users.Add(_mapper.Map<User>(user));
                }
                Debug.WriteLine(log);
                return result;
            }
            catch (Exception ex)
            {
                log.LogType = LogType.ERROR;
                Debug.WriteLine(log);
                return new ReturnUsers
                {
                    Error = $"{ex.Message}",
                    Log = log.ToString()

                };
            }
        }

        public override async Task<ReturnUser> GetUser(ReturnId request, ServerCallContext _)
        {
            var id = request.Id;
            var log = new Log(LogType.INFO, "GET user");
            try
            {
                if (id != null)
                {
                    var user = await _users.GetByIdWithCarsAsync(id);
                    Debug.WriteLine(log);
                    return new ReturnUser
                    {
                        User = _mapper.Map<User>(user),
                        Log = log.ToString()
                    };
                }
                else
                {
                    log.LogType = LogType.ERROR;
                    Debug.WriteLine(log);
                    return new ReturnUser
                    {
                        Error = "ID is null or empty.",
                        Log = log.ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                log.LogType = LogType.ERROR;
                Debug.WriteLine(log);
                return new ReturnUser
                {
                    Error = $"{ex.Message}",
                    Log = log.ToString()
                };
            }
        }

        //create or update user -cars
        public override async Task<ReturnResult> CreateOrUpdateUser(ReturnUser request, ServerCallContext context)
        {
            var data = request.User;
            var log = new Log(LogType.INFO, "POST/PUT user");

            if (request.User != null)
            {
                try
                {
                    Models.User user = new Models.User
                    {
                        FirstName = data.FirstName,
                        LastName = data.LastName,
                        Email = data.Email,
                        Password = data.Password
                    };

                    if (data.Id != "")
                    {
                        user.Id = data.Id;
                        await _users.UpdateUserAsync(data.Id, user);
                        log.HttpCall = "PUT user";
                    }
                    else
                    {
                        await _users.CreateUserAsync(user);
                        log.HttpCall = "POST user";
                    }

                    Debug.WriteLine(log);

                    return new ReturnResult
                    {
                        Success = true,
                        Log = log.ToString()
                    };
                }
                catch (Exception ex)
                {
                    log.LogType = LogType.ERROR;
                    Debug.WriteLine(log);

                    return new ReturnResult
                    {
                        Success = false,
                        Error = $"{ex.Message}",
                        Log = log.ToString()
                    };
                }
            }
            else
            {
                log.LogType = LogType.WARN;
                Debug.WriteLine(log);

                return new ReturnResult
                {
                    Error = "Request body is null or empty.",
                    Log = log.ToString()
                };
            }
        }

        // deletes user and their cars
        public override async Task<ReturnResult> DeleteUser(ReturnId request, ServerCallContext context)
        {
            var id = request.Id;
            var log = new Log(LogType.INFO, "DELETE user");

            if (id != null)
            {
                try
                {
                    var user = await _users.GetByIdWithCarsAsync(id);
                    foreach (var car in user.CarsList)
                    {
                        await _users.DeleteCarAsync(car.Id);
                    }
                    await _users.DeleteUserAsync(id);
                    Debug.WriteLine(log);

                    return new ReturnResult
                    {
                        Success = true,
                        Log = log.ToString()
                    };
                }
                catch (Exception ex)
                {
                    log.LogType = LogType.ERROR;
                    Debug.WriteLine(log);

                    return new ReturnResult
                    {
                        Success = false,
                        Error = $"{ex.Message}",
                        Log = log.ToString()
                    };
                }
            }
            else
            {
                log.LogType = LogType.WARN;
                Debug.WriteLine(log);

                return new ReturnResult
                {
                    Error = "ID is null or empty.",
                    Log = log.ToString()
                };
            }
        }

        public override async Task<ReturnResult> CreateOrUpdateUserCar(ReturnUSerIdWithCar request, ServerCallContext context)
        {
            var userId = request.UserId;
            var data = request.Car;
            var log = new Log(LogType.INFO, "POST/PUT car");

            try
            {
                var user = await _users.GetByIdWithCarsAsync(userId);

                Models.Car car = new Models.Car
                {
                    Type = data.Type,
                    Registration = data.Registration,
                    Manufacturer = data.Manufacturer,
                    Model = data.Model,
                    Country = data.Country

                };

                if (data.Id != "") //Update user car
                {
                    foreach (var item in user.CarsList)
                    {
                        if (item.Id == data.Id)
                        {
                            car.Id = data.Id;
                            await _users.UpdateCarAsync(data.Id, car);
                            log.HttpCall = "PUT car";
                        }
                        else
                        {
                            log.LogType = LogType.ERROR;
                            Debug.WriteLine(log);
                            return new ReturnResult
                            {
                                Success = false,
                                Log = log.ToString()
                            };
                        }
                    }
                }
                else //Insert user car
                {
                    var newCar = await _users.CreateCarAsync(car);
                    user.Cars.Add(newCar.Id);
                    await _users.UpdateUserAsync(userId, user);
                    log.HttpCall = "POST car";
                }

                Debug.WriteLine(log);

                return new ReturnResult
                {
                    Success = true,
                    Log = log.ToString()
                };
            }
            catch (Exception ex)
            {

                return new ReturnResult
                {
                    Success = false,
                    Error = $"{ex.Message}",
                    Log = log.ToString()
                };
            }
        }

        //deletes user's car
        public override async Task<ReturnResult> DeleteUserCar(ReturnUSerIdCarId request, ServerCallContext context)
        {
            var userId = request.UserId;
            var carId = request.CarId;
            var log = new Log(LogType.INFO, "DELETE car");

            try
            {
                var user = await _users.GetByIdWithCarsAsync(userId);
                foreach (var item in user.Cars)
                {
                    if (item == carId)
                    {
                        await _users.DeleteCarAsync(carId);
                        Debug.WriteLine(log);

                        return new ReturnResult
                        {
                            Success = true,
                            Log = log.ToString()
                        };
                    }
                }
                log.LogType = LogType.WARN;
                Debug.WriteLine(log);

                return new ReturnResult
                {
                    Success = false,
                    Log = log.ToString()
                };
            }
            catch (Exception ex)
            {
                log.LogType = LogType.ERROR;
                Debug.WriteLine(log);

                return new ReturnResult
                {
                    Success = false,
                    Error = $"{ex.Message}",
                    Log = log.ToString()
                };
            }
        }
    }
}
