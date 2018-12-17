using System.Collections.Generic;

namespace MobiliTreeApi.Domain
{
    public class ServiceProfile
    {
        public IList<TimeslotPrice> WeekDaysPrices { get; set; }
        public IList<TimeslotPrice> OverrunWeekDaysPrices { get; set; }
        public IList<TimeslotPrice> WeekendPrices { get; set; }
        public IList<TimeslotPrice> OverrunWeekendPrices { get; set; }
    }

    public class TimeslotPrice
    {
        public TimeslotPrice(int startHour, int endHour, decimal pricePerHour)
        {
            StartHour = startHour;
            EndHour = endHour;
            PricePerHour = pricePerHour;
        }

        public int StartHour { get; set; }
        public int EndHour { get; set; }
        public decimal PricePerHour { get; set; }
    }
}