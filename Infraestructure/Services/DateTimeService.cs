using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;

namespace Infrastructure.Services
{
    public class DateTimeService :IDateTimeService
    {
        public string ParsedAsMySql(DateTime date)
        {
            var d = $"{date.Month:D2}/{date.Day:D2}/{date.Year:D4}";
            return d;
        }
        public List<DateTime> GetDateTimesInBetween(DateTime startDate, DateTime endDate)
        {
            var listOfMonth = new List<DateTime>();
            var firstDate = new DateTime(startDate.Year, startDate.Month, 1);
            var lastDate = new DateTime(endDate.Year, endDate.Month, 1);
            for (DateTime i = firstDate; i <= lastDate; i = i.AddMonths(1))
            {
                listOfMonth.Add(i);
            }
            return listOfMonth;
        }
        public int IndexOf(List<DateTime> dateTimes, DateTime date)
        {
            var index = 0;
            foreach (var item in dateTimes)
            {
                if (item.Year == date.Year && item.Month == date.Month)
                    return index;
                index++;
            }
            return -1;
        }
    }
}
