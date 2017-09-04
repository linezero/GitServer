using System.Linq;
using GitServer.Services;
using GitServer.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using GitServer.ApplicationCore.Interfaces;
using GitServer.ApplicationCore.Models;
using System.IO;
using System;

namespace GitServer.Controllers
{
    [Authorize]
    public class HomeController : GitControllerBase
    {
        private IRepository<Repository> _repository;

        public HomeController(
            IOptions<GitSettings> gitOptions,
            GitRepositoryService repoService,
            IRepository<Repository> repository
        )
            : base(gitOptions, repoService)
        {
            _repository = repository;
        }

        public IActionResult Home()
        {
            var username = HttpContext.User.Identity.Name;
            var reps = _repository.List(r => r.UserName == username).ToList();
            return View(reps);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string name, string remoteurl, string description)
        {
            LibGit2Sharp.Repository result = null;
            name = name.Trim();
            var username = HttpContext.User.Identity.Name;
            var reps = _repository.List(r => r.UserName == username).ToList();
            if (reps.Count > 9)
                return View(new { error = "已超过10个限制" });
            if (reps.Exists(r => r.Name == name))
                return View(new { error = "已存在仓库" });
            if (!string.IsNullOrEmpty(name) && string.IsNullOrEmpty(remoteurl))
            {
                result = RepositoryService.CreateRepository(Path.Combine(username, name));
            }
            else if (!string.IsNullOrEmpty(remoteurl))
            {
                remoteurl = remoteurl.Trim();
                result = RepositoryService.CreateRepository(Path.Combine(username, name), remoteurl);
            }
            if (result != null)
            {
                var rep = new Repository()
                {
                    Name = name,
                    Description = description,
                    CreationDate = DateTime.Now,
                    DefaultBranch = "master",
                    UserName = username,
                    UpdateTime = DateTime.Now
                };
                _repository.Add(rep);
                return Redirect("/");
            }
            return View();
        }
    }
}