using System.Security.Cryptography;
using System.Text;
using WSOA.Server.Business.Resources;
using WSOA.Server.Data.Interface;
using WSOA.Shared.Entity;
using WSOA.Shared.Exceptions;

namespace WSOA.Server.Business.Utils
{
    public static class SecurityUtil
    {
        /// <summary>
        /// Convert string to SHA256 hash.
        /// </summary>
        public static string ToSha256(this string toHash)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(toHash);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }

        /// <summary>
        /// Check if user can perform action and return sub section performing.
        /// </summary>
        public static MainNavSubSection CanUserPerformAction(this ISession session, IMenuRepository menuRepository, int subSectionId)
        {
            string profileCode = session.GetCurrentProfileCode();

            MainNavSubSection? subSection = menuRepository.GetMainNavSubSectionByIdAndProfileCode(profileCode, subSectionId);
            if (subSection == null)
            {
                string errorMsg = MainBusinessResources.USER_CANNOT_PERFORM_ACTION;
                throw new FunctionalException(errorMsg, string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, errorMsg));
            }

            return subSection;
        }

        /// <summary>
        /// Check if user can perform action.
        /// </summary>
        public static void CanUserPerformAction(this ISession session, IUserRepository userRepository, string businessActionCode)
        {
            string profileCode = session.GetCurrentProfileCode();

            bool canPerform = userRepository.ExistsBusinessActionByProfileCode(profileCode, businessActionCode);
            if (!canPerform)
            {
                string errorMsg = MainBusinessResources.USER_CANNOT_PERFORM_ACTION;
                throw new FunctionalException(errorMsg, string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, errorMsg));
            }
        }

        /// <summary>
        /// Get the current User ID.
        /// </summary>
        public static int GetCurrentUserId(this ISession session)
        {
            string? currentUserId = session.GetString(HttpSessionResources.KEY_USER_ID);
            if (currentUserId == null)
            {
                string errorMsg = MainBusinessResources.USER_NOT_CONNECTED;
                throw new FunctionalException(errorMsg, string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, errorMsg));
            }
            return int.Parse(currentUserId);
        }

        /// <summary>
        /// Get current profile code.
        /// </summary>
        private static string GetCurrentProfileCode(this ISession session)
        {
            string? profileCode = session.GetString(HttpSessionResources.KEY_PROFILE_CODE);
            if (string.IsNullOrWhiteSpace(profileCode))
            {
                string errorMsg = MainBusinessResources.USER_NOT_CONNECTED;
                throw new FunctionalException(errorMsg, string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, errorMsg));
            }
            return profileCode;
        }
    }
}
