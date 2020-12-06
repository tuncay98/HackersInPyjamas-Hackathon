using HackerInPyjamas.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HackerInPyjamas.Controllers
{
    public class BotFunction
    {
        private readonly HackersInPyjamasContext db;

        public BotFunction()
        {
            db = new HackersInPyjamasContext();
        }
        public async Task<string> googleSearch(string des)
        {
            var url = "https://www.google.com/search?newwindow=1&sxsrf=ALeKk018LD9SyEOTu8Qey7u1mAnjYYbOIQ%3A1607096182679&source=hp&ei=dlfKX6XvJtu1tAbH4o-oDQ&q="+des+"&oq="+des+"&gs_lcp=CgZwc3ktYWIQAzIECAAQCjIECAAQCjIECAAQCjIECAAQCjIECAAQCjIGCAAQChAeMgYIABAKEB4yBggAEAoQHjIECAAQHjIICAAQBRAKEB46AggAOgIILjoHCC4QChCTAlC_hQZYhokGYK6LBmgCcAB4AIAB4gGIAZYKkgEDMi02mAEAoAEBqgEHZ3dzLXdperABAA&sclient=psy-ab&ved=0ahUKEwilgtjo07TtAhXbGs0KHUfxA9UQ4dUDCAc&uact=5";
            var web = new HtmlWeb();
            var doc = web.Load(url);

            /*var nodes = doc.DocumentNode.Descendants("span")
            .Select(y => y.Descendants()
            .Where(x => x.Attributes["class"].Value == "aCOpRe"))
            .First().First().InnerText;
            */

            return doc.ParsedText;
        }

        public async Task<int> specificWebsiteSearch(string website, string path, string mainClass, string searchClass , int pageIndex)
        {

            HtmlWeb web = new HtmlWeb();
            web.OverrideEncoding = Encoding.UTF8;
            HtmlDocument document = web.Load(website + path);

            HtmlNodeCollection coll = document.DocumentNode.SelectNodes($"//div[contains(@class, '{searchClass}')] //a[@href]");

            if (coll != null)
            {
                HtmlNode[] nodes = coll.ToArray();

                if (!nodes.Any(w=> w.GetAttributeValue("href", String.Empty).Length > 3 && w.GetAttributeValue("href", String.Empty).StartsWith('/')))
                {
                    return 0;
                }

                foreach (string href in nodes.Select(w => w.GetAttributeValue("href", String.Empty)))
                {
                    
                    //string href = item.GetAttributeValue("href", String.Empty);

                    if (href.StartsWith('/') && href.Length > 3)
                    {
                        HtmlDocument linkDoc = web.Load(website + href);

                        HtmlNode node = linkDoc.DocumentNode.SelectSingleNode($"//*[contains(@class, '{mainClass}')]");

                        if (node != null && node.InnerText.Split("  ").Any(w => w.Length > 200))
                        {
                            if (!db.IndexedLinks.Where(w => w.Link == web + href).Any())
                            {
                                IndexedLink link = new IndexedLink
                                {
                                    Link = website + href,
                                    Text = HttpUtility.HtmlDecode(String.Join(' ', node.InnerText.Split("  ").Select(w => w.Trim())).ToLower()),
                                    Date = DateTime.Now,
                                    PageIndex = pageIndex
                                };
                                db.IndexedLinks.Add(link);
                                await db.SaveChangesAsync();
                            }

                        }

                    }
                }
                return 1;
            }
            return 0;
        }

    }
}
