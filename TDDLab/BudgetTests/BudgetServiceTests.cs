﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void NoBudgetAnd20180101To20180101_0()
        {
            SetupBudgeData(new List<Budget>());
            GivenTwoDate_BudgetTotalAmountShouldBe(new DateTime(2018, 1, 1), new DateTime(2018, 1, 1), 0);
        }

        [TestMethod()]
        public void Jan31And20180101To20180101_1()
        {
            List<Budget> budgets = new List<Budget>()
            {
                new Budget(){ YearMonth = "201801", Amount = 31 }
            };

            SetupBudgeData(budgets);
            GivenTwoDate_BudgetTotalAmountShouldBe(new DateTime(2018, 1, 1), new DateTime(2018, 1, 1), 1);
        }

        [TestMethod()]
        public void Jan31And20180101To20180102_2()
        {
            List<Budget> budgets = new List<Budget>()
            {
                new Budget(){ YearMonth = "201801", Amount = 31 }
            };

            SetupBudgeData(budgets);
            GivenTwoDate_BudgetTotalAmountShouldBe(new DateTime(2018, 1, 1), new DateTime(2018, 1, 2), 2);
        }

        [TestMethod()]
        public void Jan62And20180101To20180101_2()
        {
            List<Budget> budgets = new List<Budget>()
            {
                new Budget(){ YearMonth = "201801", Amount = 62 }
            };

            SetupBudgeData(budgets);
            GivenTwoDate_BudgetTotalAmountShouldBe(new DateTime(2018, 1, 1), new DateTime(2018, 1, 1), 2);
        }

        [TestMethod()]
        public void Jan31Feb280And20180131To20180201_11()
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
        public void Jan31Feb280Mar3100And20180101To20180331_3411()
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