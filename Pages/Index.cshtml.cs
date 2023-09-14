using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DrivinEmail.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var file = Request.Form.Files.First();
            
            if(file.ContentType != "text/plain")
            {
                ModelState.AddModelError("Arquivo", "Esse arquivo precisa ser de tipo TXT.");
                return Page();
            }
            return Page();
        }

        public void OnGet()
        {

        }
    }
}