using System;

namespace OnlineTest.Base.Extensions
{
    public static class StringExtension
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return (str == null || str == string.Empty) ? true : false;
        }

        public static bool IsNull(this string str)
        {
            return (str == null) ? true : false;
        }
        public static bool IsEmpty(this string str)
        {
            return (str == string.Empty) ? true : false;
        }
    }
}
