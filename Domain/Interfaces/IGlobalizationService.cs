using System.Globalization;

namespace Domain.Interfaces
{
    public interface IGlobalizationService
    {
        CultureInfo GetCultureInfo();
        RegionInfo GetRegionInfo();
    }
}