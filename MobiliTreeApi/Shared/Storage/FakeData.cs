using System;
using System.Collections.Generic;
using MobiliTreeApi.Events.Models;
using MobiliTreeApi.Shared.Domain;

namespace MobiliTreeApi.Shared.Storage;

public static class FakeData
{
    /// <summary>
    /// Returns seed sessions for demo/testing purposes.
    /// This is used by FakeBaseSessionStorage to pre-populate the storage.
    /// In production, the database would start empty and sessions would be added via the API.
    /// -- It's only here to facilitate the exercise, does not need to be investigated --
    /// </summary>
    public static List<Session> GetSeedSessions() => [];

    /// <summary>
    /// Returns seed customer data for demo/testing purposes.
    /// This is used by fake repositories and tests.
    /// </summary>
    public static Dictionary<string, Customer> GetSeedCustomers() =>
        new()
        {
            {
                "c001", new Customer("c001", "John", "pf001")
            },
            {
                "c002", new Customer("c002", "Sarah", "pf001", "pf002")
            },
            {
                "c003", new Customer("c003", "Andrea", "pf002")
            },
            {
                "c004", new Customer("c004", "Peter")
            }
        };

    /// <summary>
    /// Returns seed service profile data for demo/testing purposes.
    /// This is used by ParkingFacilityRepository (fake implementation).
    /// </summary>
    public static Dictionary<string, ServiceProfile> GetSeedServiceProfiles() =>
        new()
        {
            {
                "pf001",
                new ServiceProfile
                {
                    OverrunWeekDaysPrices = new List<TimeslotPrice>
                    {
                        new(0, 7, 1.5m), // betweeen midnight and 7 AM price is 1.5 eur/hour
                        new(7, 18, 3.5m), // betweeen 7 AM and 6 PM price is 3.5 eur/hour
                        new(18, 24, 2.5m) // betweeen 6 PM and midnight price is 2.5 eur/hour
                    },
                    OverrunWeekendPrices = new List<TimeslotPrice>
                    {
                        new(0, 7, 1.8m),
                        new(7, 18, 3.8m),
                        new(18, 24, 2.8m)
                    },
                    WeekDaysPrices = new List<TimeslotPrice>
                    {
                        new(0, 7, 0.5m),
                        new(7, 18, 2.5m),
                        new(18, 24, 1.5m)
                    },
                    WeekendPrices = new List<TimeslotPrice>
                    {
                        new(0, 7, 0.8m),
                        new(7, 18, 2.8m),
                        new(18, 24, 1.8m)
                    },
                }
            },
            {
                "pf002",
                new ServiceProfile
                {
                    OverrunWeekDaysPrices = new List<TimeslotPrice>
                    {
                        new(0, 8, 1.5m),
                        new(8, 17, 3.5m),
                        new(17, 24, 2.5m)
                    },
                    OverrunWeekendPrices = new List<TimeslotPrice>
                    {
                        new(0, 8, 1.8m),
                        new(8, 17, 3.8m),
                        new(17, 24, 2.8m)
                    },
                    WeekDaysPrices = new List<TimeslotPrice>
                    {
                        new(0, 8, 0.5m),
                        new(8, 17, 2.5m),
                        new(17, 24, 1.5m)
                    },
                    WeekendPrices = new List<TimeslotPrice>
                    {
                        new(0, 8, 0.8m),
                        new(8, 17, 2.8m),
                        new(17, 24, 1.8m)
                    },
                }
            }
        };
}
