using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ScratchNet.Test
{
    [TestClass]
    public class BroadcastParserTest
    {
        [TestMethod]
        public void Parse()
        {
            var str = BroadcastParser.Parse( "broadcast \"bar\"" );
            Assert.AreEqual( "bar", str );
        }

        [TestMethod]
        public void IsValid()
        {
            Assert.IsTrue( BroadcastParser.IsValid( "broadcast \"bar\"" ) );
        }

        [TestMethod]
        public void broadcastしかなくてIsValidがfalse()
        {
            Assert.IsFalse( BroadcastParser.IsValid( "broadcast " ) );
        }

        [TestMethod]
        public void sensor_updateはIsValidがfalse()
        {
            Assert.IsFalse( BroadcastParser.IsValid( "sensor-update \"piyo\" 76 " ) );
        }
    }
}
