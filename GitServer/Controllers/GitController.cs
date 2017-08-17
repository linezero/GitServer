using GitServer.Services;
using GitServer.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GitServer.Controllers
{
    public class GitController : GitControllerBase
    {
		public GitController(
			GitRepositoryService repoService,
			IOptions<GitSettings> gitOptions
		)
			: base(gitOptions, repoService)
		{ }

        [Route("git/{repoName}.git/git-upload-pack")]
		public IActionResult ExecuteUploadPack(string repoName) => TryGetResult(repoName, () => GitUploadPack(repoName));

        [Route("git/{repoName}.git/git-receive-pack")]
        public IActionResult ExecuteReceivePack(string repoName) => TryGetResult(repoName, () => GitReceivePack(repoName));

        [Route("git/{repoName}.git/info/refs")]
        public IActionResult GetInfoRefs(string repoName, string service) => TryGetResult(repoName, () => GitCommand(repoName, service, true));
    }
}