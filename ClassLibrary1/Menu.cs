using System;

namespace ClassLibrary1
{
    public class Menu
    {
        public string InputCheck()
        {
            Console.WriteLine("Kies een website om te scrapen (1:YouTube 2:Jobsite 3:IMDB)");
            var selection = Console.ReadLine();
            while (string.IsNullOrEmpty(selection) || !Int32.TryParse(selection, out int result))
            {
                Console.WriteLine("uw input was niet geldig");
                Console.WriteLine("Kies een website om te scrapen (1:YouTube 2:Jobsite 3:IMDB)");
                selection = Console.ReadLine();
            }
            return selection;
        }

        public int Options()
        {
            var selection = InputCheck();
            switch (Int32.Parse(selection))
            {
                case 1:
                    return 1;
                case 2:
                    return 2;
                case 3:
                    return 3;
                default:
                    Console.WriteLine("uw input was niet geldig");
                    this.Options();
                    return 0;
            }
        }
    }
}
