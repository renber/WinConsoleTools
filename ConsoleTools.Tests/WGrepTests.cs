using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using wgrep;

namespace ConsoleTools.Tests
{
    [TestClass]
    public class WGrepTests
    {
        [TestMethod]
        public void TestMatcher_StartsWith()
        {
            WGrepArgs args = new WGrepArgs();
            args.SearchExpression = "HalloWelt";
            args.SearchMode = SearchMode.StartsWith;
            String test = "    HalloWelt      ";
            Matcher m = new Matcher(args);

            Assert.IsTrue(m.Match(test).Count > 0);
        }

        [TestMethod]
        public void TestMatcher_EndsWith()
        {
            WGrepArgs args = new WGrepArgs();
            args.SearchExpression = "HalloWelt";
            args.SearchMode = SearchMode.EndsWith;
            String test = "    HalloWelt      ";
            Matcher m = new Matcher(args);

            Assert.IsTrue(m.Match(test).Count > 0);
        }
    }
}
