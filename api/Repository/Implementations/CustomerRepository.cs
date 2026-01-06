using HealthInsuranceApi.Data;
using HealthInsuranceApi.Models;
using HealthInsuranceApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthInsuranceApi.Repositories.Implementations
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerProfile?> GetByUserIdAsync(string userId)
        {
            return await _context.CustomerProfiles
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<CustomerProfile?> GetByIdAsync(int customerProfileId)
        {
            return await _context.CustomerProfiles
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CustomerProfileId == customerProfileId);
        }

        public async Task<IEnumerable<CustomerProfile>> GetAllAsync()
        {
            return await _context.CustomerProfiles
                .Include(c => c.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<CustomerProfile>> GetInactiveCustomersAsync()
        {
            return await _context.CustomerProfiles
                .Where(c => !c.IsActive)
                .Include(c => c.User)
                .ToListAsync();
        }

        public async Task AddAsync(CustomerProfile customer)
        {
            await _context.CustomerProfiles.AddAsync(customer);
        }

        public Task UpdateAsync(CustomerProfile customer)
        {
            _context.CustomerProfiles.Update(customer);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
