using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ScratchNet.Test
{
    [TestClass]
    public class BroadcastBuilderTest
    {
        [TestMethod]
        public void Constract()
        {
            var target = new BroadcastBuilder();
            Assert.AreEqual( "broadcast", target.Message );
        }

        [TestMethod]
        public void AddMessage()
        {
            var target = new BroadcastBuilder();
            target.AddValue( "hoge" );
            Assert.AreEqual( "broadcast \"hoge\"", target.Message );
        }

        [TestMethod]
        public void Clear()
        {
            var target = new BroadcastBuilder();
            target.AddValue( "hoge" );
            Assert.AreEqual( "broadcast \"hoge\"", target.Message );

            target.Clear();
            Assert.AreEqual( "broadcast", target.Message );
        }
    }
}
