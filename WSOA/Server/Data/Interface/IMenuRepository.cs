using WSOA.Shared.Entity;

namespace WSOA.Server.Data.Interface
{
    public interface IMenuRepository
    {
        /// <summary>
        /// Get all main nav section.
        /// </summary>
        List<MainNavSection> GetMainNavSections();

        /// <summary>
        /// Get all sub sections by profile of the user connected.
        /// </summary>
        List<MainNavSubSection> GetMainNavSubSectionsByProfileCode(string profileCode);

        /// <summary>
        /// Get a sub section by ID and profile code. Return null if not found.
        /// </summary>
        MainNavSubSection? GetMainNavSubSectionByIdAndProfileCode(string profileCode, int id);
    }
}
