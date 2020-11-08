using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace cw1
{
    class MainClass
    {
        static async Task Main(string[] args)
        {
            //args = new string[] { "https://www.pja.edu.pl" }; //testing
            var url = "";
            if (args.Length > 0)
            {
                url = args[0];
            } else
            {
                throw new ArgumentNullException();
            }
            var urlCorrectlyFormatted = Uri.IsWellFormedUriString(url, UriKind.Absolute);
            if (!urlCorrectlyFormatted)
            {
                throw new ArgumentException();
            }
            var client = new HttpClient();
            try
            {
                var result = await client.GetAsync(url);

                if (result.IsSuccessStatusCode)
                {
                    string html = await result.Content.ReadAsStringAsync();
                    var matches = Regex.Matches(html, "[a-z]+[a-z0-9]*@[a-z0-9]+(\\.[a-z]*){1,}", RegexOptions.IgnoreCase)
                                        .OfType<Match>()
                                        .Select(m => m.Groups[0].Value)
                                        .Distinct();
                    var emailsFound = matches.Count() > 0;

                    if (emailsFound)
                    {
                        foreach (var i in matches)
                        {
                            Console.WriteLine(i);
                        }
                    } else {
                        Console.WriteLine("Nie znaleziono adresow email");
                    }
                }
           
                
            } catch (Exception e)
            {
                Console.WriteLine("Błąd w czasie pobierania strony");
            } finally
            {
                client.Dispose();
            }
        }
    }
}
