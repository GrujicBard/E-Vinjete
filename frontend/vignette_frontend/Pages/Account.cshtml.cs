using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Threading.Tasks;
using VignetteAuth.Protos;

namespace vignette_frontend.Pages
{
    public class AccountModel : PageModel
    {
        [BindProperty]
        public UpdatePassword UpdatePassword { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        public new Models.User User { get; set; }
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

            if (reply.User != null)
            {
                User = new()
                {
                    FirstName = reply.User.FirstName,
                    LastName = reply.User.LastName,
                    Email = reply.User.Email
                };
            }
            if (reply.Error != "")
            {
                Debug.WriteLine("Reply: " + reply.Error);
            }

            return null;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5001");
            var client = new UserService.UserServiceClient(channel);

            userId = HttpContext.Session.GetString("UserId");

            var reply = await client.GetUserAsync(
                new ReturnId() { Id = userId }
            );

            if (reply.Error != "")
            {
                Debug.WriteLine("Reply: " + reply.Error);
            }
            if (reply.User != null)
            {
                if (UpdatePassword.OldPassword == reply.User.Password)
                {
                    var updatedUser = reply.User;
                    updatedUser.Password = UpdatePassword.NewPassword;

                    var replyUpdateUser = await client.CreateOrUpdateUserAsync(
                        new ReturnUser() { User = updatedUser }
                    );

                    if (replyUpdateUser.Success)
                    {
                        StatusMessage = "Password updated.";
                    }

                    if (replyUpdateUser.Error != "")
                    {
                        Debug.WriteLine("Reply: " + replyUpdateUser.Error);
                        StatusMessage = "Reply: " + replyUpdateUser.Error;
                    }

                }
                else
                {
                    StatusMessage = "Old password is incorrect.";
                }
            }

            return RedirectToPage("Account");
        }
    }

    public class UpdatePassword
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

    }
}
