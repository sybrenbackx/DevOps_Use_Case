using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.IO;
using Newtonsoft.Json;


namespace ClassLibrary1
{
    public class IMDb_Scraper
    {
        public string str_title { get; set; }
        public string str_year { get; set; }
        public string str_link { get; set; }
        public string str_description { get; set; }
        public string str_duration { get; set; }
        public string str_genre { get; set; }
        public string str_rating { get; set; }
        public string str_actor { get; set; }

        public string GetSearchInput()
        {
            Console.WriteLine("Vul hier uw zoekterm in:");
            return Console.ReadLine();
        }

        public void SearchMovie(string search_input)
        {
            IWebDriver driver = new FirefoxDriver();
            driver.Url = "https://www.imdb.com/find/?q=" + search_input + "&ref_=nv_sr_sm";
            driver.Manage().Window.Maximize();
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            Thread.Sleep(5000);
            String separator = ";";
            StringBuilder outputcsv = new StringBuilder();
            StringBuilder outputjson = new StringBuilder();
            String[] headings = { "Title", "Year", "Link", "Description", "Duration", "Genre", "Rating", "Actors" };
            outputcsv.AppendLine("IMDb Scraper");
            outputcsv.AppendLine(string.Join(separator, headings));

            try
            {
                By movies_list = By.CssSelector("li.ipc-metadata-list-summary-item.ipc-metadata-list-summary-item--click.find-result-item.find-title-result");
                ReadOnlyCollection<IWebElement> movies = driver.FindElements(movies_list);
                for (int i = 0; i < 5; i++)
                {
                    IWebElement movie_title = movies[i].FindElement(By.CssSelector("a.ipc-metadata-list-summary-item__t"));
                    str_title = movie_title.Text;

                    IWebElement movie_year = movies[i].FindElement(By.CssSelector("span.ipc-metadata-list-summary-item__li"));
                    str_year = movie_year.Text;

                    IWebElement movie_link = movies[i].FindElement(By.CssSelector("a.ipc-metadata-list-summary-item__t"));
                    str_link = movie_link.GetAttribute("href");

                    //driver.Navigate().GoToUrl(str_link);
                    //driver.FindElement(By.XPath("/html/body/div[2]/div/div/div[2]/div/button[2]")).Click();
                    //movie_link.Click();

                    js.ExecuteScript("arguments[0].click(); ", movie_link);
                    Thread.Sleep(5000);

                    IWebElement movie_description = driver.FindElement(By.XPath("/html/body/div[2]/main/div/section[1]/section/div[3]/section/section/div[3]/div[2]/div[1]/section/p/span[3]"));
                    str_description = movie_description.Text;

                    IWebElement movie_duration = driver.FindElement(By.XPath("/html/body/div[2]/main/div/section[1]/section/div[3]/section/section/div[2]/div[1]/ul/li[last()]"));
                    str_duration = movie_duration.Text;

                    ReadOnlyCollection<IWebElement> movie_genre = driver.FindElements(By.CssSelector("span.ipc-chip__text"));
                    str_genre = "";
                    foreach (var item in movie_genre)
                    {
                        str_genre += item.Text + " ";
                    }

                    try
                    {
                        IWebElement movie_rating = driver.FindElement(By.CssSelector("span.sc-bde20123-1.cMEQkK"));
                        str_rating = movie_rating.Text + "/10";
                    }
                    catch (NoSuchElementException) { str_rating = "Unrated"; }


                    ReadOnlyCollection<IWebElement> movie_actor = driver.FindElements(By.CssSelector("a.sc-bfec09a1-1.gCQkeh"));
                    str_actor = "|";
                    foreach (var item in movie_actor)
                    {
                        str_actor += item.Text + "|";
                    }

                    driver.Navigate().Back();
                    movies = driver.FindElements(movies_list);

                    Console.WriteLine("Movie Title: " + str_title);
                    Console.WriteLine("Movie Year: " + str_year);
                    Console.WriteLine("Movie Link: " + str_link);
                    Console.WriteLine("Movie Description: " + str_description);
                    Console.WriteLine("Movie Duration: " + str_duration);
                    Console.WriteLine("Movie genre: " + str_genre);
                    Console.WriteLine("Movie rating: " + str_rating);
                    Console.WriteLine("Movie actors: " + str_actor);
                    Console.WriteLine("\n");

                    String[] newLine = { str_title, str_year, str_link, str_description, str_duration, str_genre, str_rating, str_actor };
                    outputcsv.AppendLine(string.Join(separator, newLine));

                    string jsonString = JsonConvert.SerializeObject(this, Formatting.Indented);
                    outputjson.AppendLine(jsonString);
                }
                driver.Quit();
                outputcsv.AppendLine("");
                outputjson.AppendLine("");
                File.AppendAllText("../../data/Results.csv", outputcsv.ToString());
                File.AppendAllText("../../data/Results.JSON", outputjson.ToString());
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Uw zoekterm leverde geen resultaten op.");
                this.SearchMovie(this.GetSearchInput());
            }
        }
    }
}
