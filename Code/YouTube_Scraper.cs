using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading;

namespace ClassLibrary1
{
    public class YT_Scraper
    {
        public string str_title {  get; set; }
        public string str_views { get; set; }
        public string str_link { get; set; }
        public string str_channel { get; set; }

        public string GetSearchInput()
        {
            Console.WriteLine("Vul hier uw zoekterm in:");
            return Console.ReadLine();
        }

        public void SearchYt(string search_input)
        {
            IWebDriver driver = new FirefoxDriver();
            driver.Url = "https://www.youtube.com/results?search_query=" + search_input;

            Thread.Sleep(5000);

            String separator = ";";
            StringBuilder outputcsv = new StringBuilder();
            StringBuilder outputjson = new StringBuilder();
            String[] headings = { "Title", "Channel", "Views", "Link" };
            outputcsv.AppendLine("YouTube Scraper");
            outputcsv.AppendLine(string.Join(separator, headings));

            By elem_video_link = By.CssSelector("ytd-video-renderer.style-scope.ytd-item-section-renderer");
            ReadOnlyCollection<IWebElement> videos = driver.FindElements(elem_video_link);
            Console.WriteLine("Total number of videos in " + search_input + " are " + videos.Count);
            for (int i = 0; i < 5; i++)
            {
                IWebElement elem_video_title = videos[i].FindElement(By.CssSelector("#video-title"));
                str_title = elem_video_title.Text;

                IWebElement elem_video_channel = videos[i].FindElement(By.CssSelector("#channel-info"));
                str_channel = elem_video_channel.Text;

                IWebElement elem_video_views = videos[i].FindElement(By.XPath(".//*[@id='metadata-line']/span[1]"));
                str_views = elem_video_views.Text;

                IWebElement elem_video_lnk = videos[i].FindElement(By.CssSelector("#title-wrapper h3 a"));
                str_link = elem_video_lnk.GetAttribute("href");

                Console.WriteLine("Video Title: " + str_title);
                Console.WriteLine("Video Channel: " + str_channel);
                Console.WriteLine("Video Views: " + str_views);
                Console.WriteLine("Video Link: " + str_link);
                Console.WriteLine("\n");

                String[] newLine = { str_title, str_channel, str_views, str_link };
                outputcsv.AppendLine(string.Join(separator, newLine));

                string jsonString = JsonConvert.SerializeObject(this, Formatting.Indented);
                outputjson.AppendLine(jsonString);

            }
            outputcsv.AppendLine("");
            outputjson.AppendLine("");
            File.AppendAllText("../../data/Results.csv", outputcsv.ToString());
            File.AppendAllText("../../data/Results.JSON", outputjson.ToString());
            driver.Quit();
        }
    }
}
