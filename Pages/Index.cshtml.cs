using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace XMLParser.Pages
{
    public class IndexModel : PageModel
    {
        public List<ItemProperties> ItemsProperties { get; set; } = new List<ItemProperties>();

        public async Task OnGetAsync()
        {
            using (var client = new HttpClient())
            {
                var xmlContent = await client.GetStringAsync("http://scripting.com/rss.xml");
                var doc = XDocument.Parse(xmlContent);
                var items = doc.Descendants("item");

                foreach (var item in items)
                {
                    var itemProperties = new ItemProperties
                    {
                        Title = item.Element("title")?.Value,
                        Description = item.Element("description")?.Value,
                        PubDate = item.Element("pubDate")?.Value,
                        Link = item.Element("link")?.Value,
                    };

                    ItemsProperties.Add(itemProperties);
                }
            }
        }
    }

    public class ItemProperties
    {
        public string? Title { get; set; }
        public string Description { get; set; }
        public string PubDate { get; set; }
        public string Link { get; set; }
    }
}
