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
            if (_budgetRepo.GetAll().Count > 0)
            {
                double days = (end - start).TotalDays + 1;
                return days;
            }

            return 0;
        }
    }

    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }
}
