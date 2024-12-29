using Consume.Models;
using Consume.ViewModels.Accounts;
using Consume.ViewModels.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Consume.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<HomeController> _logger;
        private HttpClient _client;
        public HomeController(ILogger<HomeController> logger,IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _client = _clientFactory.CreateClient("Pustok");
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            //_client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer",HttpContext.Session.GetString("token"));//1.sxem 2.token value

            var response =await _client.GetAsync(_client.BaseAddress+"/manage/categories?page=1");//HttpResponseMessage
            
            var responseStr =await response.Content.ReadAsStringAsync();
            //CategoryListViewModel categoryListViewModel = new CategoryListViewModel();
            CategoryListViewModel categoryListViewModel = JsonConvert.DeserializeObject<CategoryListViewModel>(responseStr);
            //if (response.StatusCode==System.Net.HttpStatusCode.OK)
            //{
            //    categoryListViewModel= JsonConvert.DeserializeObject<CategoryListViewModel>(responseStr);
            //}
            //else
            //{
            //    ViewBag.Error = "Unauthorized";
            //}
            return View(categoryListViewModel);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var loginContent = JsonConvert.SerializeObject(login);
            var requestContent = new StringContent(loginContent, Encoding.UTF8, "application/json");
            var response =await _client.PostAsync(_client.BaseAddress+"/manage/accounts/login",requestContent);
            //responsun status codelarina baxmaq lazm
            var responseStr =await response.Content.ReadAsStringAsync();
            LoginResponseViewModel loginResponseViewModel = JsonConvert.DeserializeObject<LoginResponseViewModel>(responseStr);
            //HttpContext.Session.SetString("token", loginResponseViewModel.Token);
            return Ok(loginResponseViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
