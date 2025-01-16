using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace llibrary.Guards
{
    public static class Guard
    {
        public static T AgainstNull<T>(
            [NotNull] T? argument,
            [CallerArgumentExpression(nameof(argument))]
            string? paramName = null
        )
            where T : class
        {
            if (argument == null)
            {
                throw new ArgumentNullException(paramName);
            }

            return argument;
        }
    }
}