using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ScratchNet.Test
{
    [TestClass]
    public class SensorUpdateBuilderTest
    {
        [TestMethod]
        public void Constract()
        {
            var target = new SensorUpdateBuilder();
            Assert.AreEqual( "sensor-update", target.Message );
        }

        [TestMethod]
        public void AddValue()
        {
            var target = new SensorUpdateBuilder();
            target.AddMessage( "foo", "bar" );

            Assert.AreEqual( "sensor-update \"foo\" bar", target.Message );
        }

        [TestMethod]
        public void AddValue2()
        {
            var target = new SensorUpdateBuilder();
            target.AddMessage( "foo", "bar" );
            target.AddMessage( "hoge", "piyo" );

            Assert.AreEqual( "sensor-update \"foo\" bar \"hoge\" piyo", target.Message );
        }

        [TestMethod]
        public void Clear()
        {
            var target = new SensorUpdateBuilder();
            target.AddMessage( "foo", "bar" );

            Assert.AreEqual( "sensor-update \"foo\" bar", target.Message );

            target.Clear();
            Assert.AreEqual( "sensor-update", target.Message );
        }
    }
}
