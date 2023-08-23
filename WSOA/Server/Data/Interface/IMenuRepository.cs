using WSOA.Shared.Entity;

namespace WSOA.Server.Data.Interface
{
    public interface IMenuRepository
    {
        /// <summary>
        /// Get a sub section by ID and profile code. Return null if not found.
        /// </summary>
        MainNavSubSection? GetMainNavSubSectionByIdAndProfileCode(string profileCode, int id);

        /// <summary>
        /// Get main nav section of the user comparing to his profile code and the sub sections assiociated.
        /// </summary>
        IDictionary<MainNavSection, List<MainNavSubSection>> GetMainNavSubSectionsInSectionByProfileCode(string profileCode);
    }
}
