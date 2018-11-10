using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget
{
    public class BudgetService
    {
        private IBudgetRepo _budgetRepo;

        /// <summary>
        /// 建構式 DI 注入 budgetRepo。
        /// </summary>
        /// <param name="budgetRepo"></param>
        public BudgetService(IBudgetRepo budgetRepo)
        {
            this._budgetRepo = budgetRepo;
        }

        /// <summary>
        /// 回傳時間區間內的預算加總。
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public double TotalAmount(DateTime start, DateTime end)
        {
            double amount = 0;

            // 取得所有月預算。
            var budgets = _budgetRepo.GetAll();

            if (budgets.Count == 0)
            {
                return amount;
            }


            foreach (var budget in budgets)
            {
                var budgetMonthFirstDate = GetBudgetMonthFirstDate(budget.YearMonth);
                var budgetMonthEndDate = budgetMonthFirstDate.AddMonths(1).AddDays(-1);

                // 針對有落在查詢區間的預算進行加總
                if (IsBudgetMonthIntersectWithQueryInterval(start, end, budgetMonthFirstDate, budgetMonthEndDate))
                {
                    var budgetPerDay = budget.Amount / budgetMonthEndDate.Day;
                    var days = GetIntersectiveDaysOfBudgetAndQuery(start, end, budgetMonthFirstDate, budgetMonthEndDate);
                    amount += days * budgetPerDay;
                }
            }

            return amount;
        }

        /// <summary>
        /// 取得查詢區間與預算區間的交集。
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="budgetMonthFirstDate"></param>
        /// <param name="budgetMonthEndDate"></param>
        /// <returns></returns>
        private int GetIntersectiveDaysOfBudgetAndQuery(DateTime start, DateTime end, DateTime budgetMonthFirstDate, DateTime budgetMonthEndDate)
        {
            int days = 0;
            while (budgetMonthFirstDate <= budgetMonthEndDate)
            {
                if (budgetMonthFirstDate >= start && budgetMonthFirstDate <= end)
                {
                    days++;
                }

                budgetMonthFirstDate = budgetMonthFirstDate.AddDays(1);
            }

            return days;
        }

        /// <summary>
        /// 判斷預算所在的月份是否與查詢區間有交集。
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="budgetStart"></param>
        /// <param name="budgetEnd"></param>
        /// <returns></returns>
        private bool IsBudgetMonthIntersectWithQueryInterval(DateTime start, DateTime end, DateTime budgetStart, DateTime budgetEnd)
        {
            return budgetStart >= start || budgetEnd <= end;
        }

        /// <summary>
        /// 取得 Budge 年月字串所代表月份的第一天 DateTime 物件。
        /// </summary>
        /// <param name="budgetYearMonth"></param>
        /// <returns></returns>
        private DateTime GetBudgetMonthFirstDate(string budgetYearMonth)
        {
            int year = Int32.Parse(budgetYearMonth.Substring(0, 4));
            int month = Int32.Parse(budgetYearMonth.Substring(4));

            var budgetMonthFirstDate = new DateTime(year, month, 1);
            return budgetMonthFirstDate;
        }
    }



    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }
}
