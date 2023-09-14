using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DrivinEmail.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public bool _isFileValid;
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnPost()
        {
            var file = Request.Form.Files.First();
            
            if(file.Length > 0 && file.ContentType != "text/plain")
            {
                _isFileValid = false;

                return Page();
            }
            
            _isFileValid = true;
            TempData["file"] = new StreamReader(file.OpenReadStream()).ReadToEnd();
            return RedirectToPage("/Arquivos");
        }

        public void OnGet()
        {
        }
    }
}