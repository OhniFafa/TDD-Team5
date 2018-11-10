using Microsoft.VisualStudio.TestTools.UnitTesting;
using Budget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Tests
{
    [TestClass()]
    public class BudgetServiceTests
    {
        private BudgetService _budgetService;

        [TestInitialize]
        public void Setup()
        {
            _budgetService = new BudgetService();
        }

        [TestMethod()]
        public void NoBudget_20180101()
        {
            DateTime start = new DateTime(2018, 1, 1);
            DateTime end = new DateTime(2018, 1, 1);

            double result = _budgetService.TotalAmount(start, end);

            Assert.AreEqual(0, result);
        }


    }
}