using GitHubApi;
using GitHubPortfolio.Service;
using GithubService;
using GithubService.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GitHubPortfolio.API.Controllers
{
    [ApiController]
    [Route("api/github")]
    public class GitHubController : ControllerBase
    {
        private readonly IGitHubService _gitHubService;

        public GitHubController(IGitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }

        [HttpGet("repositories/{username}")]
        public async Task<IActionResult> GetRepositories()
        {
            var repositories = await _gitHubService.GetUserRepositories();
            return Ok(repositories);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<RepoInfo>>> SearchRepositories([FromBody] SearchRepository searchRepo)
        {
            var res = await _gitHubService.SearchRepositories(searchRepo.Name, searchRepo.Language, searchRepo.UserName);
            return Ok(res);
        }
    }
}
