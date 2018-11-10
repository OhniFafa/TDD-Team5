using Microsoft.VisualStudio.TestTools.UnitTesting;
using Budget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace Budget.Tests
{
    [TestClass()]
    public class BudgetServiceTests
    {
        private BudgetService _budgetService;

        private void Setup(IBudgetRepo budgeRepo)
        {
            _budgetService = new BudgetService(budgeRepo);
        }


        [TestMethod()]
        public void NoBudget_20180101To20180101()
        {
            var stub = new Mock<IBudgetRepo>();
            stub.Setup(x => x.GetAll()).Returns(new List<Budget>());

            Setup(stub.Object);
            DateTime start = new DateTime(2018, 1, 1);
            DateTime end = new DateTime(2018, 1, 1);

            double result = _budgetService.TotalAmount(start, end);

            Assert.AreEqual(0, result);
        }

        [TestMethod()]
        public void Budget_Jan31_20180101To20180101()
        {
            var stub = new Mock<IBudgetRepo>();
            stub.Setup(x => x.GetAll()).Returns(new List<Budget>()
            {
                new Budget()
                {
                    YearMonth = "201801", 
                    Amount = 31
                }
            });

            Setup(stub.Object);
            DateTime start = new DateTime(2018, 1, 1);
            DateTime end = new DateTime(2018, 1, 1);

            double result = _budgetService.TotalAmount(start, end);

            Assert.AreEqual(1, result);
        }
    }
}