using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MotelManagement.Business.IService;
using MotelManagement.Common;
using MotelManagement.Data.Models;
using System.Text.Json;

namespace MotelManagement.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _service;
        private readonly ILogger<LoginModel> _logger;
        public LoginModel(IUserService userService, ILogger<LoginModel> logger)
        {
            this._service = userService;
            this._logger = logger;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostLoginAsync(string email, string password)
        {
            try
            {
                User user = await _service.Login(email, password);
                if (user != null)          
                {
                    string json = JsonUtil.SerializeObject(user);
                    HttpContext.Session.SetString("user", json);
                    return Redirect("~/room/list");
                }
            }catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            TempData["Message"] = "Error";
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostRegisterAsync(User user)
        {
            Console.WriteLine(JsonSerializer.Serialize(user));
            try
            {
                await _service.Register(user);
                TempData["Message"] = "Success";
            }catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                TempData["Message"] = "Failed";
            }
            return Redirect("~/Index");
        }
    }
}
