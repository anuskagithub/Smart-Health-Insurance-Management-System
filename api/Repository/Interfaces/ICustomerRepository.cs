using HealthInsuranceApi.Models;


namespace HealthInsuranceApi.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<CustomerProfile?> GetByUserIdAsync(string userId);
        Task<CustomerProfile?> GetByIdAsync(int customerProfileId);

        Task<IEnumerable<CustomerProfile>> GetAllAsync();
        Task<IEnumerable<CustomerProfile>> GetInactiveCustomersAsync();

        Task AddAsync(CustomerProfile customer);
        Task UpdateAsync(CustomerProfile customer);
        Task SaveChangesAsync();
    }
}
