using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryDatabase;

namespace LibraryDatabase.Tests
{
    [TestClass]
    public class WorkDayTests
    {
        private static WorkDay Make(int day, int issued, int returned)
        {
            return new WorkDay("Міська бібліотека", "вул. Головна, 1",
                               new DateTime(2026, 9, day), issued, returned);
        }
        
        
        [TestMethod]
        public void CountDaysIssuedMoreThanReturned_StartData_ReturnsFour()
        {
          
            List<Library> records = new List<Library>
            {
                Make(1, 24, 15),
                Make(2, 31, 18),
                Make(3, 20, 21), 
                Make(4, 16, 11),
                Make(5, 28, 13)
            };

            IBookStatistics stat = new WorkDay();

            Assert.AreEqual(4, stat.CountDaysIssuedMoreThanReturned(records));
        }

    
    }
}