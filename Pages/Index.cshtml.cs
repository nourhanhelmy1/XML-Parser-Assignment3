using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml;
using System.Xml.Linq;
using System.Net.Http;

namespace XMLParser.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public List<ItemProperties> ItemsProperties { get; set; } = new List<ItemProperties>();

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await FetchXmlContentAsync(httpClient, "http://scripting.com/rss.xml");

            if (response.IsSuccessStatusCode)
            {
                var xmlContent = await response.Content.ReadAsStringAsync();
                ItemsProperties = ParseXmlContent(xmlContent);

                return Page();
            }
            else
            {
                return RedirectToPage("/Error");
            }
        }

        async Task<HttpResponseMessage> FetchXmlContentAsync(HttpClient httpClient, string url)
        {
            return await httpClient.GetAsync(url);
        }

        List<ItemProperties> ParseXmlContent(string xmlContent)
        {
            var itemPropertiesList = new List<ItemProperties>();
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

                itemPropertiesList.Add(itemProperties);
            }

            return itemPropertiesList;
        }
    }

    public class ItemProperties
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string PubDate { get; set; }
        public string Link { get; set; }
    }
}
