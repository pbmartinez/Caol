using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IDateTimeService
    {
        string ParsedAsMySql(DateTime date);
        List<DateTime> GetDateTimesInBetween(DateTime startDate, DateTime endDate);
        int IndexOf(List<DateTime> dateTimes, DateTime date);
    }
}
