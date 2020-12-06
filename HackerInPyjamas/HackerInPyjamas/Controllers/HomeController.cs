using F23.StringSimilarity;
using HackerInPyjamas.Hubs;
using HackerInPyjamas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HackerInPyjamasML.Model;
using System.Threading;

namespace HackerInPyjamas.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHubContext<DataFlow> _hubContext;
        private readonly HackersInPyjamasContext db;
        private readonly Methods methods;
        private ModelInput input = new ModelInput();
        public HomeController(ILogger<HomeController> logger, IHubContext<DataFlow> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
            db = new HackersInPyjamasContext();
            methods = new Methods(db);
        }

        public async Task<IActionResult> Index()
        {
            
            return View();
        }

        [Route("/recieve")]
        [HttpPost]
        public async Task<IActionResult> RecieveData([FromBody] Message msg)
        {
            //input.Sentences = msg.Description.Substring(0, 10);

            //ModelOutput result = ConsumeModel.Predict(input);
           

                await _hubContext.Clients.All.SendAsync("ReceiveMessage",
                    new
                    {
                        isReported = methods.isReported(msg),
                        Index = msg.Index,
                        ReportAmount = methods.getAmount(msg),
                        Percentage = methods.calculateReliability(msg)
                    }
                );
            

            return Ok();
        }

        [Route("/Sources")]
        [HttpPost]
        public List<IndexedLink> Sources(string message)
        {
            var list = from item in db.IndexedLinks.AsParallel().WithDegreeOfParallelism(3) select item;

            var specificSources = list.Where(w => methods.isSimilarTexts(message, w.Text)).ToList();

            return specificSources;
        }

        [Route("/Source")]
        [HttpPost]
        public string Source(string message)
        {
            var list = from item in db.IndexedLinks.AsParallel().WithDegreeOfParallelism(3) select item;

            var specificSources = list.Where(w => methods.isSimilarTexts(message, w.Text)).First().Link;

            return specificSources;
        }

        [Route("/report")]
        [HttpPost]
        public async Task<IActionResult> Reported([FromBody] Message msg)
        {

            var reportDB = db.ReportedPosts.Where(w => w.Title == msg.Title && w.Description == msg.Description);
            if (reportDB.Any())
            {
                if (db.UserReports.Where(w => w.UserId == msg.Id && w.ReportedPostId == reportDB.First().Id).Any())
                    return BadRequest();

                UserReport report = new UserReport()
                {
                    UserId = msg.Id,
                    ReportedPostId = reportDB.First().Id
                };

                db.UserReports.Add(report);
                await db.SaveChangesAsync();

                await send(msg);
                return Ok();
            }

            ReportedPost post = new ReportedPost
            {
                Title = msg.Title,
                Description = msg.Description
            };
            db.ReportedPosts.Add(post);

            await db.SaveChangesAsync();

            UserReport post_report = new UserReport()
            {
                UserId = msg.Id,
                ReportedPostId = post.Id
            };

            db.UserReports.Add(post_report);

            await db.SaveChangesAsync();

            await send(msg);
            return Ok();
        }

        private async Task send(Message msg)
        {

            await _hubContext.Clients.All.SendAsync("ReceiveMessage",
                    new
                    {
                        isReported = methods.isReported(msg),
                        Index = msg.Index,
                        ReportAmount = methods.getAmount(msg),
                        Percentage = methods.calculateReliability(msg)
                    }
                );
        }
        
    }

    public class Message
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Index { get; set; }
    }
}
