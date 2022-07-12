using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using VignetteAuth.Protos;

namespace vignette_frontend.Pages
{
    public class CarsModel : PageModel
    {
        [BindProperty]
        public Car Car { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        public List<Models.Car> Cars { get; set; }
        private string userId;

        public async Task<IActionResult> OnGetAsync()
        {
            userId = HttpContext.Session.GetString("UserId");

            if (userId == null) return RedirectToPage("Login");

            using var channel = GrpcChannel.ForAddress("http://localhost:5001");
            var client = new UserService.UserServiceClient(channel);

            var reply = await client.GetUserAsync(
                new ReturnId() { Id = userId }
            );

            if (reply.User.Cars != null)
            {
                Cars = new();

                foreach(var item in reply.User.Cars)
                {
                    Models.Car car = new()
                    {
                        Registration = item.Registration,
                        Type = item.Type,
                        Country = item.Country,
                        Manufacturer = item.Manufacturer,
                        Model = item.Model
                    };
                    Cars.Add(car);
                }
            }
            if (reply.Error != "")
            {
                Debug.WriteLine("Reply: " + reply.Error);
            }

            return null;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            userId = HttpContext.Session.GetString("UserId");

            using var channel = GrpcChannel.ForAddress("http://localhost:5001");
            var client = new UserService.UserServiceClient(channel);

            var reply = await client.CreateOrUpdateUserCarAsync(
            new ReturnUSerIdWithCar
            {
                UserId = userId,
                Car = new Car
                {
                    Type = Car.Type,
                    Registration = Car.Registration,
                    Manufacturer = Car.Manufacturer,
                    Model = Car.Model,
                    Country = Car.Country
                    }
                }
            );

            if (reply.Success)
            {
                Debug.WriteLine("Reply: " + reply.Success);
                StatusMessage = "New car with registration "+Car.Registration+" added.";
            }

            if (reply.Error != "")
            {
                Debug.WriteLine("Reply: " + reply.Error);
                StatusMessage = "Error adding car.";
            }

            return RedirectToPage("Cars");
        }
    }
}
