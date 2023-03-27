using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MotelManagement.Business.IService;

namespace MotelManagement.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IRoomTypeService _roomTypeService; 
        public IndexModel(ILogger<IndexModel> logger, IRoomTypeService roomTypeService)
        {
            _logger = logger;
            _roomTypeService = roomTypeService;
        }

        public IActionResult OnGet()
        {
            return Redirect("/room/list");
        }
    }
}