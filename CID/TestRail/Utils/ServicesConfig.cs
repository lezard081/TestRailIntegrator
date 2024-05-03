using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID.TestRail.Utils
{
    public static class ServicesConfig
    {
        private static readonly ServiceCollection _services = new ServiceCollection();

        public static IServiceProvider GetServiceProvider()
        {
            ConfigureServices();

            ServiceProvider serviceProvider = _services.BuildServiceProvider();
            return serviceProvider;
        }

        private static void ConfigureServices()
        {
            _services.AddHttpClient("TestRailPost", options =>
            {
                options.BaseAddress = new Uri($"https://tr.corp.frontier.co.uk/index.php");
                options.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.Default.GetBytes($"jmanzanilla@frontier.co.uk:9dEaVMBVmDUxv2Sda56t-eOHhHriq6xPQUImJiQV2")));
            });

            _services.AddHttpClient("TestRailGet", options =>
            {
                options.BaseAddress = new Uri($"https://tr.corp.frontier.co.uk/index.php");
                options.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.Default.GetBytes($"jmanzanilla@frontier.co.uk:9dEaVMBVmDUxv2Sda56t-eOHhHriq6xPQUImJiQV2")));
            });
        }
    }
}
