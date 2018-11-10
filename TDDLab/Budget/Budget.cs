using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget
{
    public class Budget
    {
        /// <summary>
        /// 預算對應的月份(格式: yyyyMM)
        /// </summary>
        public string YearMonth { get; set; }

        /// <summary>
        /// 預算數字
        /// </summary>
        public int Amount { get; set; }
    }
}
