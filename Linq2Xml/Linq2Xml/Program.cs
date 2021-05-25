using System;
using System.Collections.Generic;

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
        public static void Main(string[] args)
        {
        }

        public static string TransformIntoXmlString(Cd cd)
        {
            return "";
        }
    }
}
