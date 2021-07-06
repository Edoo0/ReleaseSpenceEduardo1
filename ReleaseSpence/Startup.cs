using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Microsoft.Owin;
using Owin;
using scheduled.tasks.BackgroundJobs;
using System;
using System.Collections.Generic;
using System.Linq;

[assembly: OwinStartupAttribute(typeof(ReleaseSpence.Startup))]
namespace ReleaseSpence
{
    public partial class Startup
    {
        private IEnumerable<IDisposable> GetHangfireServers()
        {
            var options = new BackgroundJobServerOptions
            {
                WorkerCount = 1,
                ServerName = "gmstechs",
                Queues = new string[] { "gmstask" },
                ShutdownTimeout = TimeSpan.FromMinutes(10)
            };

            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage("Server=DESKTOP-GOCCMQ8;Database=scheduledtasks4hangfire;User=sa;Password=123456;", new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(30),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(30),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                });

            yield return new BackgroundJobServer(options);
        }

        public void Configuration(IAppBuilder app)
        {
            var filter = new BasicAuthAuthorizationFilter(
            new BasicAuthAuthorizationFilterOptions
            {
                Users = new[]
                {
                    new BasicAuthAuthorizationUser
                    {
                        Login = "admin",
                        PasswordClear = "admin"
                    }
                }
            });

            app.UseHangfireAspNet(GetHangfireServers);
            app.UseHangfireDashboard();

            var servers = JobStorage.Current.GetMonitoringApi().Servers().ToList();

            servers.ForEach(s => JobStorage.Current.GetConnection().RemoveServer(s.Name));

            _BackgroundJobs.Loader();

            ConfigureAuth(app);
        }
    }
}
