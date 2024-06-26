﻿using WSOA.Server.Data.Interface;
using WSOA.Shared.Entity;

namespace WSOA.Server.Data.Implementation
{
    public class BonusTournamentRepository : IBonusTournamentRepository
    {
        private readonly WSOADbContext _dbContext;

        public BonusTournamentRepository(WSOADbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<BonusTournament> GetAll()
        {
            return _dbContext.BonusTournaments;
        }

        public BonusTournament GetBonusTournamentByCode(string code)
        {
            return _dbContext.BonusTournaments.Single(b => b.Code == code);
        }

        public IEnumerable<BonusTournament> GetBonusTournamentsByCodes(IEnumerable<string> codes)
        {
            return
                from bonus in _dbContext.BonusTournaments
                where codes.Contains(bonus.Code)
                select bonus;
        }
    }
}
