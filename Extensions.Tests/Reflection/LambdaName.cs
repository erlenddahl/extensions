using System;
using System.Linq.Expressions;
using Extensions.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.Reflection
{
    [TestClass]
    public class LambdaName
    {
        [TestMethod]
        public void GetMemberNameTests()
        {
            Assert.AreEqual("Alpha", GetName(p => p.Alpha));
            Assert.AreEqual("Beta", GetName(p => p.Beta));
            Assert.AreEqual("Charlie", GetName(p => p.Charlie));
            Assert.AreEqual("Delta", GetName(p => p.Delta));
            Assert.AreEqual("Echo", GetName(p => p.Echo));
        }

        private string GetName(Expression<Func<Tester, object>> lam)
        {
            return lam.GetMemberName();
        }

        internal class Tester
        {
            public string Alpha { get; set; }
            public bool Beta { get; set; }
            public DateTime Delta { get; set; }
            public int Echo { get; set; }
            public double Charlie { get; set; }
        }
    }
}
