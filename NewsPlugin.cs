using System.ComponentModel;
using Microsoft.SemanticKernel;
using SimpleFeedReader;

namespace  SimpleIntro 
{
    public class NewsPlugin
    {
        [KernelFunction("get_news")]
        [Description("Get the latest news from the New York Times.")]
        [return: Description("The latest news from the New York Times.")]
        public List<FeedItem> GetNews(Kernel kernel, string category) 
        {
            var reader = new FeedReader();
            return reader.RetrieveFeed($"https://rss.nytimes.com/services/xml/rss/nyt/{category}.xml").Take(5).ToList();
        }
    }
}