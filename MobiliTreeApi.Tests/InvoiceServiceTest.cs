using System;
using System.Linq;
using MobiliTreeApi.Repositories;
using MobiliTreeApi.Services;
using Xunit;

namespace MobiliTreeApi.Tests
{
    public class InvoiceServiceTest
    {
        private readonly ISessionsRepository _sessionsRepository;
        private readonly IParkingFacilityRepository _parkingFacilityRepository;
        private readonly ICustomerRepository _customerRepository;

        public InvoiceServiceTest()
        {
            _sessionsRepository = new SessionsRepositoryFake(FakeData.GetSeedSessios());
            _parkingFacilityRepository = new ParkingFacilityRepositoryFake(FakeData.GetSeedServiceProfiles());
            _customerRepository = new CustomerRepositoryFake(FakeData.GetSeedCustomers());
        }

        [Fact]
        public void GivenSessionsService_WhenQueriedForInexistentParkingFacility_ThenThrowException()
        {
            var ex = Assert.Throws<ArgumentException>(() => GetSut().GetInvoices("nonExistingParkingFacilityId"));
            Assert.Equal("Invalid parking facility id 'nonExistingParkingFacilityId'", ex.Message);
        }

        [Fact]
        public void GivenEmptySessionsStore_WhenQueriedForUnknownParkingFacility_ThenReturnEmptyInvoiceList()
        {
            var result = GetSut().GetInvoices("pf001");

            Assert.Empty(result);
        }

        [Fact]
        public void GivenOneSessionInTheStore_WhenQueriedForExistingParkingFacility_ThenReturnInvoiceListWithOneElement()
        {
            var startDateTime = new DateTime(2018, 12, 15, 12, 25, 0);
            _sessionsRepository.AddSession(new Domain.Session
            {
                CustomerId = "some customer",
                ParkingFacilityId = "pf001",
                StartDateTime = startDateTime,
                EndDateTime = startDateTime.AddHours(1)
            });

            var result = GetSut().GetInvoices("pf001");
            
            var invoice = Assert.Single(result);
            Assert.NotNull(invoice);
            Assert.Equal("pf001", invoice.ParkingFacilityId);
            Assert.Equal("some customer", invoice.CustomerId);
        }

        [Fact]
        public void GivenMultipleSessionsInTheStore_WhenQueriedForExistingParkingFacility_ThenReturnOneInvoicePerCustomer()
        {
            var startDateTime = new DateTime(2018, 12, 15, 12, 25, 0);
            _sessionsRepository.AddSession(new Domain.Session
            {
                CustomerId = "cust1",
                ParkingFacilityId = "pf001",
                StartDateTime = startDateTime,
                EndDateTime = startDateTime.AddHours(1)
            });
            _sessionsRepository.AddSession(new Domain.Session
            {
                CustomerId = "cust1",
                ParkingFacilityId = "pf001",
                StartDateTime = startDateTime,
                EndDateTime = startDateTime.AddHours(1)
            });
            _sessionsRepository.AddSession(new Domain.Session
            {
                CustomerId = "cust2",
                ParkingFacilityId = "pf001",
                StartDateTime = startDateTime,
                EndDateTime = startDateTime.AddHours(1)
            });

            var result = GetSut().GetInvoices("pf001");

            Assert.Equal(2, result.Count);
            var invoiceCust1 = result.SingleOrDefault(x => x.CustomerId == "cust1");
            var invoiceCust2 = result.SingleOrDefault(x => x.CustomerId == "cust2");
            Assert.NotNull(invoiceCust1);
            Assert.NotNull(invoiceCust2);
            Assert.Equal("pf001", invoiceCust1.ParkingFacilityId);
            Assert.Equal("pf001", invoiceCust2.ParkingFacilityId);
            Assert.Equal("cust1", invoiceCust1.CustomerId);
            Assert.Equal("cust2", invoiceCust2.CustomerId);
        }

        [Fact]
        public void GivenMultipleSessionsForMultipleFacilitiesInTheStore_WhenQueriedForExistingParkingFacility_ThenReturnInvoicesOnlyForQueriedFacility()
        {
            var startDateTime = new DateTime(2018, 12, 15, 12, 25, 0);
            _sessionsRepository.AddSession(new Domain.Session
            {
                CustomerId = "cust1",
                ParkingFacilityId = "pf001",
                StartDateTime = startDateTime,
                EndDateTime = startDateTime.AddHours(1)
            });
            _sessionsRepository.AddSession(new Domain.Session
            {
                CustomerId = "cust1",
                ParkingFacilityId = "pf002",
                StartDateTime = startDateTime,
                EndDateTime = startDateTime.AddHours(1)
            });
            _sessionsRepository.AddSession(new Domain.Session
            {
                CustomerId = "cust2",
                ParkingFacilityId = "pf001",
                StartDateTime = startDateTime,
                EndDateTime = startDateTime.AddHours(1)
            });

            var result = GetSut().GetInvoices("pf001");

            Assert.Equal(2, result.Count);
            var invoiceCust1 = result.SingleOrDefault(x => x.CustomerId == "cust1");
            var invoiceCust2 = result.SingleOrDefault(x => x.CustomerId == "cust2");
            Assert.NotNull(invoiceCust1);
            Assert.NotNull(invoiceCust2);
            Assert.Equal("pf001", invoiceCust1.ParkingFacilityId);
            Assert.Equal("pf001", invoiceCust2.ParkingFacilityId);
            Assert.Equal("cust1", invoiceCust1.CustomerId);
            Assert.Equal("cust2", invoiceCust2.CustomerId);
        }

        private IInvoiceService GetSut()
        {
            return new InvoiceService(
                _sessionsRepository, 
                _parkingFacilityRepository,
                _customerRepository);
        }
    }
}
