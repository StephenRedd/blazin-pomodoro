using Microsoft.AspNetCore.Blazor.Browser.Rendering;
using Microsoft.AspNetCore.Blazor.Browser.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlazinPomodoro.Client
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var serviceProvider = new BrowserServiceProvider(configure => { configure.AddSingleton<TodoManager>(); });

            new BrowserRenderer(serviceProvider).AddComponent<App>("app");
        }
    }
}