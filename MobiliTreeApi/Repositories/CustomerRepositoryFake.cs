using System.Collections.Generic;
using MobiliTreeApi.Domain;

namespace MobiliTreeApi.Repositories
{
    public class CustomerRepositoryFake : ICustomerRepository
    {
        private readonly Dictionary<string, Customer> _customers;

        public CustomerRepositoryFake(Dictionary<string, Customer> customers)
        {
            _customers = customers;
        }

        public Customer GetCustomer(string customerId)
        {
            if (_customers.TryGetValue(customerId, out var customer))
            {
                return customer;    
            }

            return null;
        }
    }
}
