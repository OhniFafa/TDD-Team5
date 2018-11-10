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
        public void SameDate_NoBudget()
        {
            SetupBudgeData(new List<Budget>());
            GivenTwoDate_BudgetTotalAmountShouldBe(new DateTime(2018, 1, 1), new DateTime(2018, 1, 1), 0);
        }

        [TestMethod()]
        public void SameDate_WithBudgetInTheMonth_GreaterThan0()
        {
            List<Budget> budgets = new List<Budget>()
            {
                new Budget(){ YearMonth = "201801", Amount = 31 }
            };

            SetupBudgeData(budgets);
            GivenTwoDate_BudgetTotalAmountShouldBe(new DateTime(2018, 1, 1), new DateTime(2018, 1, 1), 1);
        }

        [TestMethod()]
        public void CrossDate_WithBudgetInTheMonth()
        {
            List<Budget> budgets = new List<Budget>()
            {
                new Budget(){ YearMonth = "201801", Amount = 31 }
            };

            SetupBudgeData(budgets);
            GivenTwoDate_BudgetTotalAmountShouldBe(new DateTime(2018, 1, 1), new DateTime(2018, 1, 2), 2);
        }

        [TestMethod()]
        public void SameDate_WithBudgetInTheMonth_GreaterThan0_2()
        {
            List<Budget> budgets = new List<Budget>()
            {
                new Budget(){ YearMonth = "201801", Amount = 62 }
            };

            SetupBudgeData(budgets);
            GivenTwoDate_BudgetTotalAmountShouldBe(new DateTime(2018, 1, 1), new DateTime(2018, 1, 1), 2);
        }

        [TestMethod()]
        public void CrossMonth_WithBudgetInTheMonths()
        {
            List<Budget> budgets = new List<Budget>()
            {
                new Budget(){ YearMonth = "201801", Amount = 31 },
                new Budget(){ YearMonth = "201802", Amount = 280 }
            };

            SetupBudgeData(budgets);
            GivenTwoDate_BudgetTotalAmountShouldBe(new DateTime(2018, 1, 31), new DateTime(2018, 2, 1), 11);
        }

        [TestMethod()]
        public void Cross3Months_WithBudgetInMonths()
        {
            List<Budget> budgets = new List<Budget>()
            {
                new Budget(){ YearMonth = "201801", Amount = 31 },
                new Budget(){ YearMonth = "201802", Amount = 280 },
                new Budget(){ YearMonth = "201803", Amount = 3100 },
            };

            SetupBudgeData(budgets);
            GivenTwoDate_BudgetTotalAmountShouldBe(new DateTime(2018, 1, 1), new DateTime(2018, 3, 31), 3411);
        }

        [TestMethod()]
        public void InvalidDateSeq()
        {
            List<Budget> budgets = new List<Budget>()
            {
                new Budget(){ YearMonth = "201801", Amount = 31 }
            };

            SetupBudgeData(budgets);
            GivenTwoDate_BudgetTotalAmountShouldBe(new DateTime(2018, 1, 2), new DateTime(2018, 1, 1), 0);
        }


        [TestMethod()]
        public void CrossYear_WithBudgetInMonths()
        {
            List<Budget> budgets = new List<Budget>()
            {
                new Budget(){ YearMonth = "201712", Amount = 31 },
                new Budget(){ YearMonth = "201801", Amount = 310 },
            };

            SetupBudgeData(budgets);
            GivenTwoDate_BudgetTotalAmountShouldBe(new DateTime(2017, 12, 31), new DateTime(2018, 1, 1), 11);
        }


        private void SetupBudgeData(List<Budget> budgets)
        {
            var stub = new Mock<IBudgetRepo>();
            stub.Setup(x => x.GetAll()).Returns(budgets);

            Setup(stub.Object);
        }

        private void GivenTwoDate_BudgetTotalAmountShouldBe(DateTime start, DateTime end, int expected)
        {
            double result = _budgetService.TotalAmount(start, end);
            Assert.AreEqual(expected, result);
        }
    }
}