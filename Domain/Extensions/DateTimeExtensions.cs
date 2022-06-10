using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ParsedAsMySql(this DateTime date)
        {
            var d = $"{date.Month:D2}/{date.Day:D2}/{date.Year:D4}";
            return d;
        }

    }
}
