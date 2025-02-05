using GithubService.Entities;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubService
{
    public interface IGitHubService
    {

        public Task<List<RepoInfo>> GetUserRepositories();

        // פונקציה שמבצעת חיפוש repositories לפי שם, שפה ומשתמש

        public Task<List<RepoInfo>> SearchRepositories(string repositoryName = "", Language language = Language.C, string username = "");
        public Task<bool> IsActive(DateTime lastUpdate);


    }
}
