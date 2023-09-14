using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MimeKit;
using Xamarin.Essentials;
using System.Text.Encodings;
using System.Text;

namespace DrivinEmail.Pages
{
    public class ArquivosModel : PageModel
    {
        public void OnGet()
        {
            var file = TempData["file"].ToString();
            var adresses = file.Split("\n").ToList();
            var uniqueAdresses = adresses.Distinct().ToList();
            if(uniqueAdresses.Count % 5 != 0)
            {
                var missingAdresses = 5 - (uniqueAdresses.Count % 5);
                for (int i = 0; i < missingAdresses; i++)
                {
                    uniqueAdresses.Add("");
                }
            }

            List<List<string>> arrangedList = new List<List<string>>();

            for (int i = 0; i < uniqueAdresses.Count; i++)
            {
                if(i % 5 == 0)
                {
                    arrangedList.Add(new List<string>());
                }
                arrangedList[arrangedList.Count - 1].Add(uniqueAdresses[i]);

            }
            var concatenatedAdressesList = new List<string>();
            for (int i = 0; i < arrangedList.Count; i++)
            {
                concatenatedAdressesList.Add(string.Join("\n", arrangedList[i]));
            }


            for (int i = 0; i < concatenatedAdressesList.Count; i++)
            {
                
            }

        }


        public void OnPost()
        {
        }
    }
}
