using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using Nancy.Json;
using vignette_frontend.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Grpc.Net.Client;
using VignetteAuth.Protos;

namespace vignette_frontend.Pages
{
    public class IndexModel : PageModel
    {
        private string userId;
        [BindProperty]
        public Report[] Reports { get; set; }
        [BindProperty]
        public string Registration { get; set; }
        [BindProperty]
        public List<string> Registrations { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            userId = HttpContext.Session.GetString("UserId");

            if (userId == null) return RedirectToPage("Login");

            await RefreshRegistrationsAsync();

            return null;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await RefreshRegistrationsAsync();

            var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:8080/validation/reg/")
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetAsync(Registration);
            JavaScriptSerializer js = new();
            Reports = js.Deserialize<Report[]>(response.Content.ReadAsStringAsync().Result);

            return null;
        }

        public async Task RefreshRegistrationsAsync()
        {
            userId = HttpContext.Session.GetString("UserId");

            Registrations = new List<string>();

            using var channel = GrpcChannel.ForAddress("http://localhost:5001");
            var grpcClient = new UserService.UserServiceClient(channel);

            var reply = await grpcClient.GetUserAsync(
                new ReturnId() { Id = userId }
            );

            foreach (var car in reply.User.Cars)
            {
                Registrations.Add(car.Registration);
            }
        }
    }
}
