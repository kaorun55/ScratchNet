using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ScratchNet.Test
{
    [TestClass]
    public class SensorUpdateParserTest
    {
        [TestMethod]
        public void Parse()
        {
            var value = SensorUpdateParser.Parse( "sensor-update \"piyo\" 76 " );
            Assert.IsTrue( value.ContainsKey( "piyo" ) );
            Assert.AreEqual( "76", value["piyo"] );
        }

        [TestMethod]
        public void Parse2()
        {
            var value = SensorUpdateParser.Parse( "sensor-update \"hoge\" 2 \"piyo\" 75 " );
            Assert.IsTrue( value.ContainsKey( "hoge" ) );
            Assert.AreEqual( "2", value["hoge"] );
            Assert.IsTrue( value.ContainsKey( "piyo" ) );
            Assert.AreEqual( "75", value["piyo"] );
        }

        [TestMethod]
        public void IsValid()
        {
            Assert.IsTrue( SensorUpdateParser.IsValid( "sensor-update \"piyo\" 76 " ) );
        }

        [TestMethod]
        public void sensor_updateしかなくてIsValidがfalse()
        {
            Assert.IsFalse( SensorUpdateParser.IsValid( "sensor_update " ) );
        }

        [TestMethod]
        public void broadcastはIsValidがfalse()
        {
            Assert.IsFalse( SensorUpdateParser.IsValid( "broadcast \"bar\"" ) );
        }
    }
}
