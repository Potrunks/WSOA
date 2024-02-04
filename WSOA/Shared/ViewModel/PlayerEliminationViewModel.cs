using WSOA.Shared.Entity;

namespace WSOA.Shared.ViewModel
{
    public class PlayerEliminationViewModel
    {
        public PlayerEliminationViewModel()
        {

        }

        public PlayerEliminationViewModel(User user, IEnumerable<Elimination> eliminations, IEnumerable<Player> players)
        {
            FullName = $"{user.FirstName} {user.LastName}";

            NbEliminationAsEliminator = eliminations.Where(eli => players
                                                                  .Where(pla => pla.UserId == user.Id)
                                                                  .Select(pla => pla.Id)
                                                                  .ToList()
                                                                  .Contains(eli.PlayerEliminatorId)).Count();

            NbEliminationAsVictim = eliminations.Where(eli => players
                                                              .Where(pla => pla.UserId == user.Id)
                                                              .Select(pla => pla.Id)
                                                              .ToList()
                                                              .Contains(eli.PlayerVictimId)).Count();

            RatioElimination = NbEliminationAsEliminator - NbEliminationAsVictim;
        }

        public int NbEliminationAsEliminator { get; set; }

        public int NbEliminationAsVictim { get; set; }

        public int RatioElimination { get; set; }

        public string FullName { get; set; }
    }
}
