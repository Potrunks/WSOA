using Microsoft.EntityFrameworkCore.Storage;

namespace WSOA.Server.Data.Interface
{
    public interface ITransactionManager
    {
        IDbContextTransaction BeginTransaction();
        void RollbackTransaction();
        void CommitTransaction();
    }
}
