using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using vignette_frontend.Models;
using VignetteAuth.Protos;

namespace vignette_frontend.Pages
{
    public class VignettesModel : PageModel
    {
        [BindProperty]
        public Vignette Vignette { get; set; }
        [BindProperty]
        [Required]
        public string Vig_Type { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        private string userId;
        public Vignette[] Vignettes { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            userId = HttpContext.Session.GetString("UserId");

            if (userId == null) return RedirectToPage("Login");

            var client = new HttpClient
            {
                BaseAddress = new System.Uri("http://localhost:8888/v1/vignette/user/")
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetAsync(userId);
            JavaScriptSerializer js = new();
            Vignettes = js.Deserialize<Vignette[]>(response.Content.ReadAsStringAsync().Result);
            return null;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            userId = HttpContext.Session.GetString("UserId");

            using var channel = GrpcChannel.ForAddress("http://localhost:5001");
            var grpcClient = new UserService.UserServiceClient(channel);

            var reply = await grpcClient.GetCarByRegAsync(
                new ReturnReg
                {
                    Registration = Vignette.registration
                }
            );

            if(reply.Car == null)
            {
                StatusMessage = "Car does not exsist in database.";
                return RedirectToPage("Vignettes");
            }

            var vignette = new Vignette
            {
                userId = userId,
                registration = Vignette.registration,
                type = Vignette.type,
                dateCreated = DateTime.UtcNow.ToString(),
            };
            var dateValid = DateTime.UtcNow;
            switch (Vig_Type)
            {
                case "0":
                    dateValid = dateValid.AddDays(7);
                    break;
                case "1":
                    dateValid = dateValid.AddDays(182);
                    break;
                case "2":
                    dateValid = dateValid.AddDays(365);
                    break;
                default:
                    break;
            }

            vignette.dateValid = dateValid.ToString();

            var jsonObject = Newtonsoft.Json.JsonConvert.SerializeObject(vignette);

            var client = new HttpClient
            {
                BaseAddress = new System.Uri("http://localhost:8888/v1/")
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.PostAsync("vignette/", new StringContent(jsonObject, Encoding.UTF32, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            return RedirectToPage("Vignettes");
        }
    }
}
