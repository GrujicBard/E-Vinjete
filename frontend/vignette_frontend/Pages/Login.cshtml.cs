using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using vignette_frontend.Models;
using System.Diagnostics;
using Grpc.Net.Client;
using VignetteAuth.Protos;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace vignette_frontend.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Credential Credential { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5001");
            var client = new UserService.UserServiceClient(channel);

            var user = await client.GetUserByEmailAsync(
                new ReturnEmail() { Email = Credential.Email }
            );

            if (user.User != null)
            {
                if (Credential.Password == user.User.Password)
                {
                    HttpContext.Session.SetString("UserId", user.User.Id);
                    return RedirectToPage("Index");
                }
                else
                {
                    StatusMessage = "Login failed.";
                    Debug.WriteLine("Login failed.");
                }
            }

            if (user.Error != "")
            {
                Debug.WriteLine("Reply: " + user.Error);
            }
            return null;
        }
    }
}
