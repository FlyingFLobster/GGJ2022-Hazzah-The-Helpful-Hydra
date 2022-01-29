using NUnit.Framework;
using System.Collections.Generic;
using Dialogue;

namespace ScriptTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            // When Hazzah enters a new zone, load all dialogue from actors who can be in that zone
            string zone_code = "zone1";
            Dictionary<string, Dictionary<string, LineData>> lines = LineLoader.GetZoneLines(zone_code);
            Assert.Pass();
        }
    }
}