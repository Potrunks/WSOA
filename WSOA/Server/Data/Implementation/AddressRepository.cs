using WSOA.Server.Data.Interface;
using WSOA.Shared.Entity;

namespace WSOA.Server.Data.Implementation
{
    public class AddressRepository : IAddressRepository
    {
        private readonly WSOADbContext _dbContext;

        public AddressRepository(WSOADbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Address> GetAllAddresses()
        {
            return
            (
                from adr in _dbContext.Addresses
                select adr
            )
            .ToList();
        }
    }
}
