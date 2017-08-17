using System.Linq;
using GitServer.Services;
using GitServer.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GitServer.Controllers
{
    public class HomeController : GitControllerBase
    {
        public HomeController(
            IOptions<GitSettings> gitOptions,
            GitRepositoryService repoService
        )
            : base(gitOptions, repoService)
        {
        }

        public IActionResult Home()
        {
            return View(RepositoryService.RepositoryDirectories.Select(d => d.Name));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string name,string remoteurl)
        {
            LibGit2Sharp.Repository result = null;
            if (!string.IsNullOrEmpty(name) && string.IsNullOrEmpty(remoteurl))
                result = RepositoryService.CreateRepository(name);
            else if (!string.IsNullOrEmpty(remoteurl))
                result = RepositoryService.CreateRepository(name, remoteurl);
            if (result != null)
                return Redirect("/");
            return View();
        }
    }
}