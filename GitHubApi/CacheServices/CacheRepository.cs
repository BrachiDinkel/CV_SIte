using GithubService.Entities;

namespace GitHubApi.CacheServices
{
    public class CacheRepository
    {
        public DateTime LastUpdate { get; set; } = DateTime.Now;
        public List<RepoInfo> Repos { get; set; }

    }
}
