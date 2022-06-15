namespace Domain.Interfaces
{
    public interface ICurrencyService
    {
        string GetCurrencyValue(decimal value);
        string GetCurrencyValue(double value);
    }
}