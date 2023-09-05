using WSOA.Shared.Exceptions;
using WSOA.Shared.Resources;

namespace WSOA.Shared.Utils
{
    public static class DateUtil
    {
        /// <summary>
        /// Check if the date is past or not.
        /// </summary>
        public static void IsAfterOrEqualUtcNow(this DateTime date)
        {
            if (date < DateTime.UtcNow.Date)
            {
                throw new FunctionalException(DataValidationResources.STARTDATE_PAST_ERROR, null);
            }
        }
    }
}
