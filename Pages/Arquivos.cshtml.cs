using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MimeKit;
using Xamarin.Essentials;
using System.Text;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http.HttpResults;
using System.IO;
using System;
using Microsoft.AspNetCore.SignalR.Protocol;
using DrivinEmail.Models;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;

namespace DrivinEmail.Pages
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class ArquivosModel : PageModel
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public List<EmailFileModel> EmailFiles;
        public ArquivosModel(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            EmailFiles = new List<EmailFileModel>();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var file = TempData["file"];
            if (TempData["file"] == null)
            {
                return RedirectToPage("index");
            }
            if (!CheckIfFileIsValid())
            {
                TempData[""] = null;
                TempData["invalidFile"] = true;
                return RedirectToPage("index");
            }
            List<string> concatenatedAdressesList = CreateFiveEmailsList();
            CreateEmailFiles(concatenatedAdressesList);

            return Page();
        }

        private bool CheckIfFileIsValid()
        {
            var file = TempData["file"].ToString();

            bool isEmail = file.Contains("@") && file.Contains(".com");

            return isEmail;
        }

        private void CreateEmailFiles(List<string> concatenatedAdressesList)
        {
            var filesPath = Directory.GetCurrentDirectory().Replace(@"\", @"/");
            System.IO.Directory.CreateDirectory(filesPath + "/EmailFiles");

            for (int i = 0; i < concatenatedAdressesList.Count; i++)
            {
                var fileName = $"email{i + 1}.txt";

                EmailFiles.Add(new EmailFileModel
                {
                    Id = i + 1,
                    Name = fileName,
                    Path = $"{filesPath}{fileName}"

                });

                var file = System.IO.File.Create(filesPath + fileName);
                file.Write(Encoding.UTF8.GetBytes(concatenatedAdressesList[i]));
                file.Close();
            }
        }


        private List<string> CreateFiveEmailsList()
        {
            var baseFile = TempData["file"].ToString();
            var adresses = baseFile.Split("\n").ToList();
            var uniqueAdresses = adresses.Distinct().ToList();
            if (uniqueAdresses.Count % 5 != 0)
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
                if (i % 5 == 0)
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

            return concatenatedAdressesList;
        }

        public async Task<IActionResult> OnPostAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return BadRequest("Invalid file path.");
            }

            try
            {
                var file = new FileInfo(filePath);
                var fileStream = file.OpenRead();
                var download = File(fileStream, "application/octet-stream", file.Name);
                return download;
            }
            catch
            {
                return Page();
            }
        }



    }
}
