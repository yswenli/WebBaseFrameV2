using System;

namespace Common
{

    public static class ExpandHelper
    {
        public static Guid ToGuid(this Guid? helper)
        {
            if (helper == null)
                helper = new Guid();
            return (Guid)helper;
        }

        public static bool ToBool(this bool? helper)
        {
            if (helper == null)
                helper = false;
            return (bool)helper;
        }

        public static string ToShortDateString(this DateTime? helper)
        {
            if (helper == null)
                return string.Empty;
            return Convert.ToDateTime(helper).ToShortDateString();
        }

    }

}
