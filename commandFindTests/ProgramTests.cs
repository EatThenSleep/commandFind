using Microsoft.VisualStudio.TestTools.UnitTesting;
using commandFind;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace commandFind.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        [TestMethod()]
        public void BuildOptionsTest()
        {
            string[] args = { "/v", "/c", "/n" };

            var options = Program.BuildOptions(args);

            Assert.IsNotNull(options);
            Assert.IsTrue(options.FindDontContain);
            Assert.IsTrue(options.CountMode);
            Assert.IsTrue(options.IsCaseSensitive);
        }
    }
}