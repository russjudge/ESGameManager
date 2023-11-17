using Microsoft.VisualStudio.TestTools.UnitTesting;
using ESGameManagerLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESGameManagerLibrary.Tests
{
    [TestClass()]
    public class GameTests
    {
        [TestMethod()]
        public void ShrinkImageIfNecessaryTest()
        {
            Game.ShrinkImageIfNecessary("E:\\DefaultUser\\Documents\\roms\\amstradcpc\\media\\screenshot\\#\\20230107_165904.jpg");
        }
    }
}