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
            string testXml = TransformIntoXmlString(createCd());
            Console.WriteLine("dit is het aanmaken");
            Console.WriteLine(testXml);
            Console.WriteLine("\n");


            HttpClient client = new();
            String response = "";
            try
            {
                response = await client.GetStringAsync(@"http://ws.audioscrobbler.com/2.0/?method=album.getinfo&album=awake&artist=Dream%20Theater&api_key=b5cbf8dcef4c6acfc5698f8709841949");
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message :{0} ", e);
            }
            Cd cd = createCd();

            XDocument lfmDoc = XDocument.Parse(response);

            var missingTracks =
                from lfmTrack in lfmDoc.Descendants("track")
                where
                    !(
                        from ownTrack in cd
                        select ownTrack.Title
                    ).Contains(lfmTrack.Element("name").Value)
                select new Track
                {
                    Artist = lfmTrack.Element("artist").Element("name").Value,
                    Title = lfmTrack.Element("name").Value,
                    Length = TimeSpan.FromSeconds(int.Parse(lfmTrack.Element("duration").Value))
                };

            foreach (Track track in missingTracks)
            {
                cd.Add(track);
            }
            Console.WriteLine(TransformIntoXmlString(cd));



        }



        public static string TransformIntoXmlString(Cd cd)
        {
            var cdXml = new XDocument();
            var rootElem = new XElement("CDs");
            cdXml.Add(rootElem);


            var cdElem = new XElement("cd");
            var cdTitleElem = new XAttribute("title", cd.Title);
            var cdArtistElem = new XAttribute("artist", cd.Artist);

            cdElem.Add(cdTitleElem);
            cdElem.Add(cdArtistElem);

            var tracksElem = new XElement("tracks");
            foreach (Track track in cd)
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
            return cdXml.ToString();
        }

        public static Cd createCd()
        {
            Cd cd = new Cd
            {
                new Track { Title = "6:00", Artist = "Dream Theater", Length = new TimeSpan(0, 5, 31)},
                new Track { Title = "Innocence Faded", Artist = "Dream Theater", Length = new TimeSpan(0, 5, 34)},
                new Track { Title = "Scarred", Artist = "Dream Theater", Length = new TimeSpan(0, 10, 59)}
            };
            cd.Title = "Awake";
            cd.Artist = "Dream Theater";


            return cd;
        }
    }
}
