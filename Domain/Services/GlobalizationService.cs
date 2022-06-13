using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class GlobalizationService : IGlobalizationService
    {
        public CultureInfo GetCultureInfo()
        {
            return new CultureInfo("pt-BR");
        }
    }
}
