using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Extensions
{
    public static class IEnumerableExtensions
    {
        public static string GetAsCsvSingleQuote(this IEnumerable<string>? source) 
        {
            if (source == null || !source.Any())
                return "''";

            var items = string.Empty;

            source?.ToList().ForEach(a =>
            {
                if (!string.IsNullOrEmpty(a))
                {
                    items = $"{items},'{a}'";
                }
            });

            if (string.IsNullOrEmpty(items))
                items = "''";
            if (items.StartsWith(','))
                items = items.Remove(0, 1);
            if (items.EndsWith(','))
                items = items.Remove(items.Length - 1);

            return items;
        }
    }
}
