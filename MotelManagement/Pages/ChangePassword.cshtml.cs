using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MotelManagement.Business.IService;
using MotelManagement.Common;
using MotelManagement.Data.Models;

namespace MotelManagement.Pages
{
    public class ChangePasswordModel : PageModel
    {
        private readonly IUserService _service;
        private readonly ILogger<ChangePasswordModel> _logger;
        public ChangePasswordModel(IUserService userService, ILogger<ChangePasswordModel> logger)
        {
            this._service = userService;
            this._logger = logger;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostChangepwAsync(string oldPassword, string password, string confirmPassword) {
            string json = HttpContext.Session.GetString("user");
            Console.WriteLine(json);
            if (json == null|| !password.Equals(confirmPassword))
            {
                TempData["Message"] = "Failed";
                return Page();
            }
            else
            {
                User user = (User)JsonUtil.DeserializeObject<User>(json);
                if (user.Password.Equals(oldPassword))
                {
                    try
                    {
                        user.Password = password;
                        await _service.ChangePasswordAync(user);
                        TempData["Message"] = "SuccChangePw";
                        return RedirectToPage("./Index");
                    }
                    catch(Exception ex)
                    {
                        _logger.LogError(ex.ToString());
                    }
                }               
            }
            TempData["Message"] = "Wrong";
            return Page();
        }    
    }
}
