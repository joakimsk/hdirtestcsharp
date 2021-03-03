using System;
using System.Net.Http;
using System.Web;
using System.Text.Json;
using System.Collections.Generic;
using System.IO; // For file IO

// See https://json2csharp.com/json-to-csharp to make class for Json Serialization

namespace hdirtestcsharp
{
    class Program
    {
        public class CovidRegistreringer
        {
            public DateTime dato { get; set; }
            public int antInnlagte { get; set; }
        }

        public class Root
        {
            public string id { get; set; }
            public string helseforetakNavn { get; set; }
            public string region { get; set; }
            public List<CovidRegistreringer> covidRegistreringer { get; set; }
            public string dokumentType { get; set; }
            public DateTime sistImportertTilHapi { get; set; }
        }

        static void Main()
        {
            Console.WriteLine("Helsedirektoratet API Test");

            const string subscription_key_secret = "enter-secret-key-here"; // Put your secret subscription key here

            MakeRequest(subscription_key_secret);

            Console.WriteLine("Push ENTER to exit");
            Console.ReadLine();
        }

        static async void MakeRequest(string secret_key)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", secret_key);

            var uri = "https://api.helsedirektoratet.no/ProduktCovid19/Covid19statistikk/helseforetak?" + queryString;

            var response = await client.GetAsync(uri);
            string resultContent = await response.Content.ReadAsStringAsync();

            Console.WriteLine(resultContent);

            // Output to file if you want to see what is received
            // File.WriteAllText("output.txt", resultContent);

            try
            {
                // I believe I need to make a JsonConverter for the List<> in the class - but how?
                Root myDeserializedClass = JsonSerializer.Deserialize<Root>(resultContent);
                //Console.WriteLine(myDeserializedClass);
            }
            catch (JsonException e)
            {
                Console.WriteLine("JSON Exception: " + e.Message);
            }
        }
    }
}
