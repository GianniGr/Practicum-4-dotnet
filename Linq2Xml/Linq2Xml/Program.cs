using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Linq2Xml
{
    public class Track
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public TimeSpan Length { get; set; }
    }

    public class Cd : List<Track>
    {
        public string Title { get; set; }
        public string Artist { get; set; }
    }
    
    public static class Program
    {
        public static async Task Main()
        {
            List<Cd> cds = createCds();
            string testXml = TransformIntoXmlString(cds);
            //Console.WriteLine(testXml);

            HttpClient client = new();
            try
            {
                String response = await client.GetStringAsync(@"http://ws.audioscrobbler.com/2.0/?method=album.getinfo&album=awake&artist=Dream%20Theater&api_key=b5cbf8dcef4c6acfc5698f8709841949");

                var xmlDoc = XDocument.Parse(response);

                var query = from tracksDoc in xmlDoc.Elements("lfm").Elements("Album").Elements("tracks")
                            select tracksDoc;

                XElement selectedTracks = query.SingleOrDefault();

                Console.WriteLine(selectedTracks);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message :{0} ", e);
            }
            catch (Exception e)
            {
                Console.WriteLine("Message :{0} ", e);
            }

    }


        public static bool loadTracks()
        {
            return true;
        }

        public static string TransformIntoXmlString(List<Cd> cds)
        {
            var cdXml = new XDocument();
            var rootElem = new XElement("CDs");
            cdXml.Add(rootElem);
            foreach (Cd cd in cds)
            {
                var cdElem = new XElement("cd");
                var cdTitleElem = new XElement("title", cd.Title);
                var cdArtistElem = new XElement("artist", cd.Artist);
                
                cdElem.Add(cdTitleElem);
                cdElem.Add(cdArtistElem);

                var tracksElem = new XElement("tracks");
                foreach(Track track in cd)
                {
                    var trackTitleElem = new XElement("title", track.Title);
                    var trackArtistElem = new XElement("artist", track.Artist);
                    var trackLengthElem = new XElement("length", track.Length.ToString());
                    var trackElem = new XElement("track");
                    
                    trackElem.Add(trackTitleElem);
                    trackElem.Add(trackArtistElem);
                    trackElem.Add(trackLengthElem);
                    tracksElem.Add(trackElem);
                }
                
                cdElem.Add(tracksElem);
                rootElem.Add(cdElem);
            }
            return cdXml.ToString();
        }

        public static List<Cd> createCds()
        {
            Cd testCD = new Cd 
            {
                new Track { Title = "6:00", Artist = "Dream Theater", Length = new TimeSpan(0, 5, 31)},
                new Track { Title = "Innocence Faded", Artist = "Dream Theater", Length = new TimeSpan(0, 5, 34)},
                new Track { Title = "Scarred", Artist = "Dream Theater", Length = new TimeSpan(0, 10, 59)}
            };
            testCD.Title = "Awake";
            testCD.Artist = "Dream Theater";

            List<Cd> cds = new List<Cd> { testCD };
            return cds;
        }
    }  
}
