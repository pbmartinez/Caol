using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IGlobalizationService _globalizationService;

        public CurrencyService(IGlobalizationService globalizationService)
        {
            _globalizationService = globalizationService ?? throw new ArgumentNullException(nameof(globalizationService));
        }

        public string GetCurrencyValue(double value)
        {
            var culture = _globalizationService.GetCultureInfo();
            return value.ToString("C", culture);
        }
        public string GetCurrencyValue(decimal value)
        {
            var culture = _globalizationService.GetCultureInfo();
            return value.ToString("C", culture);
        }
    }
}
