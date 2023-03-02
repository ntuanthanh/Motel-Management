using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MotelManagement.Common;

namespace MotelManagement.Pages
{
    public class PrivacyModel : PageModel
    {
        private UploadFileUnit upload;
        private IWebHostEnvironment webHostEnvironment;
        public PrivacyModel(UploadFileUnit upload, IWebHostEnvironment env)
        {
            this.upload = upload;
            webHostEnvironment = env;
        }

        public void OnGet()
        {
        }

        public async Task OnPost(IFormFile[] file)
        {
            await upload.UploadFile(file, "avatar", null);
        }
    }
}