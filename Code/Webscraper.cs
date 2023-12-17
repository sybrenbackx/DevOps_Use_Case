using ClassLibrary1;
using System;



namespace use_case_devops_Sybren
{
    public class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Menu start = new Menu();

                int choice = start.InputCheck();

                if (choice == 1)
                {
                    YT_Scraper scraper = new YT_Scraper();
                    Console.WriteLine("Welkom bij de YouTube webscraper");
                    scraper.SearchYt(scraper.GetSearchInput());
                }
                if (choice == 2) {
                    Job_Scraper scraper = new Job_Scraper();
                    Console.WriteLine("Welkom bij de IctJob webscraper");
                    scraper.SearchJob(scraper.GetSearchInput());
                }
                if (choice == 3) {
                    IMDb_Scraper scraper = new IMDb_Scraper();
                    Console.WriteLine("Welkom bij de IMDb webscraper");
                    scraper.SearchMovie(scraper.GetSearchInput());
                }
            }
        }
    }
}
