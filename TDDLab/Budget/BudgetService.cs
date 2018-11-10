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

        public BudgetService(IBudgetRepo budgetRepo)
        {
            this._budgetRepo = budgetRepo;
        }

        public double TotalAmount(DateTime start, DateTime end)
        {
            var budgets = _budgetRepo.GetAll();
            if (budgets.Count > 0)
            {
                Dictionary<string, int> monthDaysMap = new Dictionary<string, int>();


                while (start <= end)
                {
                    string key = start.ToString("yyyyMM");
                    if (monthDaysMap.ContainsKey(key))
                    {
                        monthDaysMap[key]++;
                    }
                    else
                    {
                        monthDaysMap.Add(key, 1);
                    }

                    start = start.AddDays(1);
                }

                Dictionary<string, int> monthPerDayBudget = budgets.ToDictionary(x => x.YearMonth,
                    x => x.Amount / _GetDaysInTheMonth(x.YearMonth));


                double result = 0;

                foreach (string monthKey in monthDaysMap.Keys)
                {

                    if (monthPerDayBudget.ContainsKey(monthKey))
                    {
                        result += monthDaysMap[monthKey] * monthPerDayBudget[monthKey];
                    }
                }

                return result;
            }

            return 0;
        }

        private int _GetDaysInTheMonth(string yyyyMM)
        {
            return new DateTime(int.Parse(yyyyMM.Substring(0, 4)),
                int.Parse(yyyyMM.Substring(4, 2)), 1).AddMonths(1).AddDays(-1).Day;
        }
    }



    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }
}
