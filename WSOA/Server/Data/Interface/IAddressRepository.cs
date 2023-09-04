using WSOA.Shared.Entity;

namespace WSOA.Server.Data.Interface
{
    public interface IAddressRepository
    {
        /// <summary>
        /// Get all addresses in DB.
        /// </summary>
        IEnumerable<Address> GetAllAddresses();
    }
}
