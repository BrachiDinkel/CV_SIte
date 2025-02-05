using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GithubService;
using GithubService.Entities; // ✅ ייבוא המרחב הנכון למחלקת RepoInfo
using Microsoft.Extensions.Options;
using Octokit;

namespace GitHubPortfolio.Service
{
    public class GitHubService : IGitHubService
    {
        private readonly GitHubClient _client;
        private readonly GitHubOption _options;

        public GitHubService(IOptions<GitHubOption> options)
        {
            _options = options.Value;
            _client = new GitHubClient(new ProductHeaderValue("GitHubPortfolio"));
            _client.Credentials = new Credentials(_options.GitHubToken);
        }

        public async Task<List<RepoInfo>> GetUserRepositories()
        {
            var repositories = await _client.Repository.GetAllForCurrent();
            var portfolio = new List<RepoInfo>();

            foreach (var repo in repositories)
            {
                if (string.IsNullOrEmpty(repo.DefaultBranch))
                {
                    continue; // מוודא שלא תהיה גישה ל-DefaultBranch ריק
                }

                var languages = await _client.Repository.GetAllLanguages(repo.Id);
                var lastCommit = await _client.Repository.Commit.Get(repo.Id, repo.DefaultBranch);
                var stars = repo.StargazersCount;
                var pullRequests = await _client.PullRequest.GetAllForRepository(repo.Owner.Login, repo.Name);
                var siteUrl = repo.HtmlUrl;

                var repoInfo = new RepoInfo
                {
                    Name = repo.Name,
                    Languages = languages.ToList(),
                    LastCommit = lastCommit?.Commit?.Author?.Date ?? DateTimeOffset.MinValue, // מונע NullReferenceException
                    Stars = stars,
                    PullRequests = pullRequests.Count,
                    SiteUrl = siteUrl
                };

                portfolio.Add(repoInfo);
            }

            return portfolio; // מחזיר List<RepoInfo>
        }


        public async Task<List<RepoInfo>> SearchRepositories(string repositoryName = "", Language language = Language.C, string username = "")
        {
            var request = new SearchRepositoriesRequest(repositoryName)
            {
                Language = language,
                User = username
            };

            var searchResults = await _client.Search.SearchRepo(request);

            return searchResults.Items.Select(repo => new RepoInfo
            {
                Name = repo.Name,
                LastCommit = repo.PushedAt ?? DateTimeOffset.MinValue,
                Stars = repo.StargazersCount,
                SiteUrl = repo.HtmlUrl
            }).ToList();
        }

        public async Task<List<string>> GetRepositoryLanguages(string owner, string repoName)
        {
            var languages = await _client.Repository.GetAllLanguages(owner, repoName);
            return languages.Select(lang => lang.Name).ToList();
        }

        public async Task<bool> IsActive(DateTime lastUpdate)
        {
            var res = await _client.Activity.Events.GetAllUserPerformed(_options.UserName);
            return true;
        }
    }
}
