using GitServer.Handlers;
using GitServer.Services;
using GitServer.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IO;

namespace GitServer.Controllers
{
    [Authorize(AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme)]
    public class GitController : GitControllerBase
    {
        public GitController(
            GitRepositoryService repoService,
            IOptions<GitSettings> gitOptions
        )
            : base(gitOptions, repoService)
        { }

        [Route("{userName}/{repoName}.git/git-upload-pack")]
        public IActionResult ExecuteUploadPack(string userName, string repoName) => TryGetResult(repoName, () => GitUploadPack(Path.Combine(userName, repoName)));

        [Route("{userName}/{repoName}.git/git-receive-pack")]
        public IActionResult ExecuteReceivePack(string userName, string repoName) => TryGetResult(repoName, () => GitReceivePack(Path.Combine(userName, repoName)));

        [Route("{userName}/{repoName}.git/info/refs")]
        public IActionResult GetInfoRefs(string userName, string repoName, string service) => TryGetResult(repoName, () => GitCommand(Path.Combine(userName, repoName), service, true));
    }
}