using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using wgrep.Arguments;

namespace ConsoleTools.Tests
{
    [TestClass]
    class ConsoleArgAttributeTests
    {

        [TestMethod]
        public void EnumArgAttributeTest()
        {
            var attr = new EnumArgAttribute(typeof(TestEnum), TestEnum.EnumVal2, "", "");
            var tc = new TestClass();

            var pi = tc.GetType().GetProperty("TestEnum", BindingFlags.Public | BindingFlags.Instance);

            attr.TryToApply(tc, pi, new List<string>() { "-3" });
            Assert.AreEqual(TestEnum.EnumVal3, tc.TestEnum);

            attr.TryToApply(tc, pi, new List<string>() { "" });
            Assert.AreEqual(TestEnum.EnumVal2, tc.TestEnum);

            try
            {
                attr.TryToApply(tc, pi, new List<string>() { "-2", "-1" });
                Assert.Fail("Expected ArgumentException not thrown");
            }
            catch (ArgumentException e)
            {
                Assert.IsTrue(e.Message.StartsWith("Contradicting"));
            }

        }
    }

    class TestClass
    {
        [EnumSwitch(TestEnum.EnumVal1, "-1", "...")]
        [EnumSwitch(TestEnum.EnumVal2, "-2", "...")]
        [EnumSwitch(TestEnum.EnumVal3, "-3", "...")]
        public TestEnum TestEnum { get; set; }
    }

    enum TestEnum
    {
        EnumVal1,
        EnumVal2,
        EnumVal3,
    }
}
