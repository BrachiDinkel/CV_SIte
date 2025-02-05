using GitHubPortfolio.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubService
{
    public static class Extensions
    {
        
        public static void AddGitHub(this IServiceCollection services, Action<GitHubOption> configureOption)
        {

            services.Configure(configureOption);
            services.AddScoped<IGitHubService,GitHubService>();
        }
    }
}
