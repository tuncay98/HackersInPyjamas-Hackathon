using F23.StringSimilarity;
using HackerInPyjamas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackerInPyjamas.Controllers
{
    public class Methods
    {
        private readonly HackersInPyjamasContext db;
        public Methods(HackersInPyjamasContext db)
        {
            this.db = db;
        }
        public int getAmount(Message msg)
        {
            var reportDB = db.ReportedPosts.Where(w => w.Title == msg.Title && w.Description == msg.Description);

            if (reportDB.Any())
                return db.UserReports.Where(w => w.ReportedPostId == reportDB.First().Id).Count();

            return 0;
        }

        public bool isReported(Message msg)
        {
            Console.WriteLine(msg.Id);
            var reportDB = db.ReportedPosts.Where(w => w.Title == msg.Title && w.Description == msg.Description);
            if (reportDB.Any() && db.UserReports.Where(w => w.UserId == msg.Id && w.ReportedPostId == reportDB.First().Id).Any())
            {
                return true;
            }

            return false;
        }

        public float calculateReliability(Message msg)
        {
            float reliability = 50;
            int amount = getAmount(msg);

            reliability -= (5 * amount);

            // In this section, Source system will affect the reliability percentage. (FOR NOW, IN THE FUTURE)

            return Math.Max(reliability, 0);
        }

        public bool isSimilarTexts(string min, string max)
        {
            var q = new NormalizedLevenshtein();

            int index = 0;
            foreach (var wordA in min.Split(' '))
            {
                foreach (var wordB in max.Split(' '))
                {
                    if (q.Similarity(wordB, wordA) > 0.6)
                    {
                        index++;
                        break;
                    }
                }
            }
            if (index > min.Split(' ').Length * 0.7)
            {
                return true;
            }

            return false;
        }

    }
}
