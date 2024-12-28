using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LLibrary.Guards
{
    public static class Guard
    {
        public static T AgainstNull<T>(
            [NotNull] T? argument,
            [CallerArgumentExpression(nameof(argument))] string? paramName = null
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
