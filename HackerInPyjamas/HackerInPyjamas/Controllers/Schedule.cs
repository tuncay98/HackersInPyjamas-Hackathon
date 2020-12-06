using HackerInPyjamas.Models;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using System.Threading.Tasks;

namespace HackerInPyjamas.Controllers
{
    public class Schedule : IHostedService
    {
        private Timer _timer;
        private readonly HackersInPyjamasContext db;

        public Schedule()
        {
            db = new HackersInPyjamasContext();
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(
                checkPoint,
                null,
                TimeSpan.Zero,
                TimeSpan.FromMinutes(30)
                );

            return Task.CompletedTask;
        }

        public async void checkPoint(object state)
        {
            var bot = new BotFunction();
            foreach (var row in db.SpecificAdresses.ToList())
            {
                for (int i = 0; i < 2; i++)
                {
                    int result = await bot.specificWebsiteSearch(website: row.Website, path: row.Path + i,
                    mainClass: row.Class, searchClass: row.SearchClass, pageIndex: i
                    );

                    if (result == 0) break;
                }
            }
            

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}
