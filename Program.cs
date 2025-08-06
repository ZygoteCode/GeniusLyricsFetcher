using FUDChromeDriver;
using System.Text.RegularExpressions;

public class Program
{
    public static void Main()
    {
        Console.Title = "GeniusLyricsFetcher V1 | Made by https://github.com/ZygoteCode/";

        UndetectedChromeDriver driver = UndetectedChromeDriver.Create(
            driverExecutablePath: $"{Environment.CurrentDirectory}\\chromedriver.exe",
            browserExecutablePath: "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe",
            headless: true
        );

        string URI = "";

        do
        {
            Console.Write("[INFO] >> Please, insert here the URL to the Genius lyrics >> ");
            URI = Console.ReadLine();

            if (!IsValidGeniusUrl(URI))
            {
                Console.WriteLine("[ERROR] >> Invalid Genius URL.");
            }
        }
        while (!IsValidGeniusUrl(URI));

        Console.WriteLine("[INFO] >> Fetching lyrics, please wait a while. They'll be saved in the file 'Lyrics.txt'.");

        try
        {
            driver.GoToUrl(URI);

            while (!driver.IsPageContentLoaded())
            {
                Thread.Sleep(1);
            }

            object result = driver.ExecuteScript(File.ReadAllText("script.js"));
            List<string> stringList = ((IEnumerable<object>)result).Cast<string>().ToList();
            string newResult = "";

            foreach (string str in stringList)
            {
                string newStr = str.Replace("<br>", "\r\n");

                if (newResult == "")
                {
                    newResult = newStr;
                }
                else
                {
                    newResult += "\r\n" + newStr;
                }
            }

            File.WriteAllText("Lyrics.txt", newResult);
            Console.WriteLine(newResult);
            Console.WriteLine("[INFO] >> The lyrics have been saved in the file 'Lyrics.txt'.");
        }
        catch
        {
            Console.WriteLine("[ERROR] >> Failed to retrieve the lyrics. Maybe the link is not valid, expired or there is a networking problem.");
        }

        Console.ReadLine();
    }

    private static bool IsValidGeniusUrl(string url)
    {
        string pattern = @"^https:\/\/genius\.com\/[^\/\s]+\/?$";
        return Regex.IsMatch(url, pattern, RegexOptions.IgnoreCase);
    }
}