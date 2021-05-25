using System;
using System.Xml;
using Linq2Xml;
using NUnit.Framework;

namespace Ling2XmlTests
{
    public class Tests
    {
        private readonly Cd _cd = new () { Title = "MyAwesomeCd" };
        
        [SetUp]
        public void Setup()
        {
            _cd.Add(new Track { Title = "AwesomeTitle1", Artist = "AwesomeArtist1", Length = TimeSpan.Zero });
            _cd.Add(new Track { Title = "AwesomeTitle2", Artist = "AwesomeArtist2", Length = TimeSpan.Zero });
            _cd.Add(new Track { Title = "AwesomeTitle3", Artist = "AwesomeArtist3", Length = TimeSpan.Zero });
        }

        [TearDown]
        public void TearDown()
        {
            _cd.Clear();
        }

        [Test]
        public void ShouldBeValidXml()
        {
            var document = new XmlDocument();
            var xmlString = Program.TransformIntoXmlString(_cd);
            
            Assert.DoesNotThrow(() => document.LoadXml(xmlString));
        }

        [Test]
        public void IsNotEmptyWhenGivingACd()
        {
            Assert.False(string.IsNullOrEmpty(Program.TransformIntoXmlString(_cd)));
        }

        [TestCase("cd")]
        [TestCase("track")]
        [TestCase("tracks")]
        [TestCase("artist")]
        [TestCase("title")]
        [TestCase("length")]
        public void ShouldContainGivenXmlElement(string elementType)
        {
            var xmlString = Program.TransformIntoXmlString(_cd);

            // Check for closing tags, in a better scenario you would probably use RegEx for this:
            // https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex?view=net-5.0
            Assert.True(xmlString.Contains($"</{elementType}>"));
        }
    }
}
