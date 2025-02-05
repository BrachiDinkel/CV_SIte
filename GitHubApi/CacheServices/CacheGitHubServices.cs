using GithubService;
using GithubService.Entities;
using Microsoft.Extensions.Caching.Memory;
using Octokit;

namespace GitHubApi.CacheServices
{
    public class CacheGitHubServices:IGitHubService
    {
        private readonly IGitHubService _gitHubService;
        private readonly IMemoryCache _memoryCache;

        private const string PortfolioKey = "PortfolioKey";

        public CacheGitHubServices(IGitHubService gitHubService, IMemoryCache memoryCache)
        {
            _gitHubService = gitHubService;
            _memoryCache = memoryCache;
        }
        public async Task<List<RepoInfo>> GetUserRepositories()
        {
            if (_memoryCache.TryGetValue(PortfolioKey, out List<RepoInfo> cacheRepos))
                return cacheRepos;

            var cachOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(30))
                .SetSlidingExpiration(TimeSpan.FromSeconds(10));

            var portfolio = await _gitHubService.GetUserRepositories();

            _memoryCache.Set(PortfolioKey, portfolio, cachOptions);
            return portfolio;
        }
        public async Task<List<RepoInfo>> SearchRepositories(string repositoryName = "", Language language = Language.C, string username = "")
        {
            return await _gitHubService.SearchRepositories(repositoryName, language, username);
        }
     

        public async Task<bool> IsActive(DateTime lastUpdate)
        {
            return await _gitHubService.IsActive(lastUpdate);
        }
    }
}
