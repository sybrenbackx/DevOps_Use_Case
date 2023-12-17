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
    public class Job_Scraper
    {
        public string str_title { get; set; }
        public string str_company { get; set; }
        public string str_link { get; set; }
        public string str_location { get; set; }
        public string str_keywords { get; set;}

        public string GetSearchInput()
        {
            Console.WriteLine("Vul hier uw zoekterm in:");
            return Console.ReadLine();
        }

        public void SearchJob(string search_input)
        {
            IWebDriver driver = new FirefoxDriver();
            driver.Url = "https://www.ictjob.be/en/search-it-jobs?keywords=" + search_input;

            Thread.Sleep(5000);

            String separator = ";";
            StringBuilder outputcsv = new StringBuilder();
            StringBuilder outputjson = new StringBuilder();
            String[] headings = { "Title", "Location", "Company", "Link", "Keywords" };
            outputcsv.AppendLine("Job Scraper");
            outputcsv.AppendLine(string.Join(separator, headings));

            try
            {
                driver.FindElement(By.Id("sort-by-date")).Click();
                Thread.Sleep(10000);
                By joblist = By.CssSelector("li.search-item.clearfix");
                ReadOnlyCollection<IWebElement> jobs = driver.FindElements(joblist);
                for (int i = 0; i < 5; i++)
                {
                    IWebElement job_title = jobs[i].FindElement(By.CssSelector("span a h2 "));
                    str_title = job_title.Text;

                    IWebElement job_location = jobs[i].FindElement(By.XPath(".//span[2]/span[2]/span[2]/span/span"));
                    str_location = job_location.Text;

                    IWebElement job_company = jobs[i].FindElement(By.CssSelector("span.job-company"));
                    str_company = job_company.Text;

                    IWebElement job_link = jobs[i].FindElement(By.CssSelector("a.job-title.search-item-link"));
                    str_link = job_link.GetAttribute("href");
                    try
                    {
                        IWebElement job_keywords = jobs[i].FindElement(By.CssSelector("span.job-keywords"));
                        str_keywords = job_keywords.Text;
                    }
                    catch (Exception) { str_keywords = " "; }
                    Console.WriteLine("Job Title: " + str_title);
                    Console.WriteLine("Job Location: " + str_location);
                    Console.WriteLine("Job Company: " + str_company);
                    Console.WriteLine("Job Link: " + str_link);
                    Console.WriteLine("Job Keywords: " + str_keywords);
                    Console.WriteLine("\n");

                    String[] newLine = { str_title, str_location, str_company, str_link, str_keywords };
                    outputcsv.AppendLine(string.Join(separator, newLine));

                    string jsonString = JsonConvert.SerializeObject(this, Formatting.Indented);
                    outputjson.AppendLine(jsonString);
                }
                driver.Quit();

                outputcsv.AppendLine("");
                outputcsv.AppendLine("");
                File.AppendAllText("../../data/Results.csv", outputcsv.ToString());
                File.AppendAllText("../../data/Results.JSON", outputjson.ToString());
            }
            catch (ArgumentOutOfRangeException) 
            {
                Console.WriteLine("Uw zoekterm leverde geen resultaten op.");
                this.SearchJob(this.GetSearchInput());
            }
        }
    }
}
