extern alias SUT;

using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Threading.Tasks;
using VignetteAuth.Protos;

namespace Tests;

[TestFixture]
public class Tests
{
    private GrpcChannel? channel;
    private UserService.UserServiceClient? client;
    private string? userId;


    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var factory = new WebApplicationFactory<SUT::Program>();

        var options = new GrpcChannelOptions { HttpHandler = factory.Server.CreateHandler() };

        channel = GrpcChannel.ForAddress(factory.Server.BaseAddress, options);

        userId = "6265176b535cdf1bef59d123";
    }

    [SetUp]
    public void Setup()
    {
        client = new UserService.UserServiceClient(channel);
    }

    [Test, Order(1)]
    public async Task TestGetUsers()
    {
        var reply = await client.GetUsersAsync(new VoidRequest());

        Assert.True(reply.Users.Count == 2, "Expected actualCount to be 2.");
    }

    [Test, Order(2)]
    public async Task TestCreateUser()
    {
        User user = new User
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@gmail.com",
            Password = "1234"
        };

        var reply = await client.CreateOrUpdateUserAsync(
            new ReturnUser() { User = user }
        );

        Assert.IsTrue(reply.Success);
    }

    [Test, Order(3)]
    public async Task TestGetUser()
    {
        var reply = await client.GetUserAsync(
            new ReturnId() { Id = userId }
        );

        Assert.That(reply.User.Id, Does.Contain(userId));
        Assert.That(reply.User.FirstName, Does.Contain("John"));
        Assert.That(reply.User.LastName, Does.Contain("Doe"));
        Assert.That(reply.User.Email, Does.Contain("john.doe@gmail.com"));
        Assert.That(reply.User.Password, Does.Contain("1234"));
    }

    [Test, Order(4)]
    public async Task TestUpdateUser()
    {
        User user = new User
        {
            Id = userId,
            FirstName = "Jane"
        };

        var replyUpdate = await client.CreateOrUpdateUserAsync(
            new ReturnUser() { User = user }
        );

        var replyGet = await client.GetUserAsync(
            new ReturnId() { Id = userId }
        );

        Assert.IsTrue(replyUpdate.Success);
        Assert.That(replyGet.User.FirstName, Does.Contain("Jane"));
    }


    [Test, Order(5)]
    public async Task TestCreateCar()
    {
        Car car = new Car
        {
            Id = "625c53ff58199221f2bf4357",
            Type = "2A",
            Registration = "BP-074",
            Manufacturer = "Mercedes",
            Model = "Delta",
            Country = "Slovenia"
        };

        var reply = await client.CreateOrUpdateUserCarAsync(
            new ReturnUSerIdWithCar
            {
                UserId = userId,
                Car = car
            }
        );

        Assert.IsTrue(reply.Success);
    }

    [Test, Order(6)]
    public async Task TestUpdateCar()
    {
        Car car = new Car
        {
            Id = "625c53ff58199221f2bf4357",
            Manufacturer = "Volkswagen"
        };

        var reply = await client.CreateOrUpdateUserCarAsync(
            new ReturnUSerIdWithCar
            {
                UserId = userId,
                Car = car
            }
        );

        Assert.IsTrue(reply.Success);
    }

    [Test, Order(7)]
    public async Task TestDeleteCar()
    {
        var reply = await client.DeleteUserCarAsync(
                new ReturnUSerIdCarId()
                {
                    UserId = userId,
                    CarId = "625c53ff58199221f2bf4357"
                }
            );

        Assert.IsTrue(reply.Success);
    }

    [Test, Order(8)]
    public async Task TestDeleteUser()
    {
        var reply = await client.DeleteUserAsync(
            new ReturnId() { Id = userId }
        );

        Assert.IsTrue(reply.Success);
    }
}