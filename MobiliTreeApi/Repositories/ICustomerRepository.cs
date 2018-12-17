using MobiliTreeApi.Domain;

namespace MobiliTreeApi.Repositories
{
    public interface ICustomerRepository
    {
        Customer GetCustomer(string customerId);
    }
}