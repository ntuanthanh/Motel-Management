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

        public void OnPost(string password) {
            string json = HttpContext.Session.GetString("user");
            User user = (User) JsonUtil.DeserializeObject(json);
            user.Password = password;
            //_service.
        }    
    }
}
