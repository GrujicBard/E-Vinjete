using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Threading.Tasks;
using vignette_frontend.Models;
using VignetteAuth.Protos;

namespace vignette_frontend.Pages
{
    public class RegistrationModel : PageModel
    {
        [BindProperty]
        public new Models.User User { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                StatusMessage = "Passwords do not match.";
                return RedirectToPage("Registration");
            }
            using var channel = GrpcChannel.ForAddress("http://localhost:5001");
            var client = new UserService.UserServiceClient(channel);

            var oldUser = await client.GetUserByEmailAsync(
                new ReturnEmail() { Email = User.Email }
            );

            if (oldUser.User == null)
            {
                VignetteAuth.Protos.User user = new()
                {
                    FirstName = User.FirstName,
                    LastName = User.LastName,
                    Email = User.Email,
                    Password = User.Password
                };

                var newUser = await client.CreateOrUpdateUserAsync(
                    new ReturnUser() { User = user }
                );

                if (newUser.Success)
                {
                    Debug.WriteLine("Reply: " + newUser.Success);
                    StatusMessage = "Registered successfully.";
                }

                if (newUser.Error != "")
                {
                    Debug.WriteLine("Reply: " + newUser.Error);
                    StatusMessage = newUser.Error.ToString();
                    return null;
                }
                return RedirectToPage("Login");
            }
            else
            {
                StatusMessage = "Email in use.";
                Debug.WriteLine("Reply: " + oldUser.User);
                Debug.WriteLine("Email in use.");
            }

            if (oldUser.Error != null)
            {
                Debug.WriteLine("Reply: " + oldUser.Error);
            }
            return null;
        }
    }
}
