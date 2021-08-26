using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace dtv_live_weather
{
    class Program
    {
        static async Task Main(string[] args)
        {

            // prep
            string userProfile = Environment.GetEnvironmentVariable("userprofile");
            string jsonURL = "http://cdn.weatherstem.com/dashboard/data/dynamic/weatherlinklive/001D0A714B3F.json";
            string jsonContent = null;
            string XMLPath = $@"{userProfile}\Desktop\WeatherStemData.xml";
            using var client = new HttpClient();
            XNode ConvertedJson = null;

            while (true)
            {
                // Make request, put data stream into jsoncontent variable

                var status = await client.GetAsync(jsonURL);
                status.EnsureSuccessStatusCode();
                jsonContent = await client.GetStringAsync(jsonURL);

                // debug shit
                Console.WriteLine(jsonContent);

                try
                {
                    // the big one
                    ConvertedJson = JsonConvert.DeserializeXNode(jsonContent, "root");
                }
                catch (Exception e)
                {
                    //uh oh spaghettio
                    Console.WriteLine("Exception caught! Details here:");
                    Console.WriteLine(e);
                    Console.WriteLine("Take a picture of these results and show them to Chris.");
                    Console.ReadKey();
                    Environment.Exit(0);
                }

                // Convert and write data to file

                Console.WriteLine("New data!");
                Console.WriteLine(ConvertedJson.ToString());
                Console.WriteLine("Writing new data.");
                File.WriteAllText(XMLPath, ConvertedJson.ToString());
                
                //Wait 15 seconds, then do it again
                Thread.Sleep(15000);
            }
        }
    }
}
