using Microsoft.EntityFrameworkCore.Storage;
using WSOA.Server.Data.Interface;

namespace WSOA.Server.Data.Implementation
{
    public class TransactionManager : ITransactionManager
    {
        private readonly WSOADbContext _dbContext;

        public TransactionManager(WSOADbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _dbContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _dbContext.Database.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            _dbContext.Database.RollbackTransaction();
        }
    }
}
