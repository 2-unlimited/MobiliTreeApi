using System.Collections.Generic;

namespace MobiliTreeApi.Shared.Domain;

public class ServiceProfile
{
    public IList<TimeslotPrice> WeekDaysPrices { get; set; }
    public IList<TimeslotPrice> OverrunWeekDaysPrices { get; set; }
    public IList<TimeslotPrice> WeekendPrices { get; set; }
    public IList<TimeslotPrice> OverrunWeekendPrices { get; set; }
}

public class TimeslotPrice(int startHour, int endHour, decimal pricePerHour)
{
    public int StartHour { get; set; } = startHour;
    public int EndHour { get; set; } = endHour;
    public decimal PricePerHour { get; set; } = pricePerHour;
}
