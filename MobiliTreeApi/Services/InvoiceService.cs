using System;
using System.Collections.Generic;
using System.Linq;
using MobiliTreeApi.Domain;
using MobiliTreeApi.Repositories;

namespace MobiliTreeApi.Services
{
    public interface IInvoiceService
    {
        List<Invoice> GetInvoices(string parkingFacilityId);
        Invoice GetInvoice(string parkingFacilityId, string customerId);
    }

    public class InvoiceService: IInvoiceService
    {
        private readonly ISessionsRepository _sessionsRepository;
        private readonly IParkingFacilityRepository _parkingFacilityRepository;
        private readonly ICustomerRepository _customerRepository;

        public InvoiceService(ISessionsRepository sessionsRepository, IParkingFacilityRepository parkingFacilityRepository, ICustomerRepository customerRepository)
        {
            _sessionsRepository = sessionsRepository;
            _parkingFacilityRepository = parkingFacilityRepository;
            _customerRepository = customerRepository;
        }

        public List<Invoice> GetInvoices(string parkingFacilityId)
        {
            var serviceProfile = _parkingFacilityRepository.GetServiceProfile(parkingFacilityId);
            if (serviceProfile == null)
            {
                throw new ArgumentException($"Invalid parking facility id '{parkingFacilityId}'");
            }

            var sessions = _sessionsRepository.GetSessions(parkingFacilityId);

            return sessions.GroupBy(x => x.CustomerId).Select(x => new Invoice
            {
                ParkingFacilityId = parkingFacilityId,
                CustomerId = x.Key,
                Amount = 0
            }).ToList();
        }

        public Invoice GetInvoice(string parkingFacilityId, string customerId)
        {
            throw new NotImplementedException();
        }
    }
}
