using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC
{
    static class EnumExtensions
    {
        /// <summary>
        /// Returns true, if ANY of the bits in flags is also set
        /// in this instance. (In contrast to HasFlag, which returns
        /// true if ALL bits are set).
        /// </summary>
        public static bool HasAnyFlag(this Enum e, Enum flag)
        {
            if (flag == null)
                throw new ArgumentNullException("flag");

            if (!e.GetType().IsEquivalentTo(flag.GetType()))
                throw new ArgumentException("The enum type mismatches.", "flag");

            return (Convert.ToUInt64(e) & Convert.ToUInt64(flag)) != 0;
        }
    }
}
