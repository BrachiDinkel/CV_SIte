using Octokit;

namespace GitHubApi
{
    public class SearchRepository
    {
        public string Name { get; set; } = "";
        public Language Language { get; set; } = Language.CSharp;
        public string UserName { get; set; } = "BrachiDinkel";
    }
}
