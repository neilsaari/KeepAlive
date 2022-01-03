using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepAlive
{
    public static class Utility
    {
        /// <summary>
        /// Test if an enumerable collection is empty.  Returns true if target
        /// null or if target does not contain any elements (zero length).
        /// </summary>
        /// <typeparam name="T">Enumerable collection element type.</typeparam>
        /// <param name="target">Enumerable collection to test.</param>
        /// <returns>True indicating collection empty.</returns>
        public static bool IsEmpty<T>(this IEnumerable<T> target)
            => (target is null) || (target.Count() == 0);

        /// <summary>
        /// Test if a string is empty.  Returns true if target null or
        /// if target does not contain any characters (zero length).
        /// </summary>
        /// <param name="target">String to test.</param>
        /// <returns>True indicating string empty.</returns>
        public static bool IsEmpty(this string target)
            => string.IsNullOrEmpty(target);

        /// <summary>
        /// Test if a string is empty.  Returns true if target empty
        /// if target only contains blanks (space, tab, new line, form feed).
        /// </summary>
        /// <param name="target">String to test.</param>
        /// <returns>True indicating string empty or blank.</returns>
        public static bool IsBlank(this string target)
            => string.IsNullOrWhiteSpace(target);

        /// <summary>
        /// Test if any processes with the specified name are running.
        /// </summary>
        /// <param name="name">Process name to look for.</param>
        /// <returns>True if at least one process with the specified name is running.</returns>
        public static bool IsRunning(string name)
        {
            Process[] processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(name));
            return (!Utility.IsEmpty(processes));
        }

        /// <summary>
        /// Functional using statement wrapper.  Wraps a using statement around the supplied function, a
        /// function of an IDisposable, and returns the result.  Creator creates the IDisposable function
        /// input parameter.  Exiting this method disposes this function input paramger.
        /// </summary>
        /// <typeparam name="TDisposable">IDisposable type.</typeparam>
        /// <typeparam name="TOut">Function result type.</typeparam>
        /// <param name="creator">TDisposable creator method.</param>
        /// <param name="function">Wrapped function.</param>
        /// <returns>Result of calling function on an IDisposable created by creator.</returns>
        public static TOut Disposer<TDisposable, TOut>(Func<TDisposable> creator, Func<TDisposable, TOut> function)
            where TDisposable : IDisposable
        {
            using (var target = creator()) {
                return function(target);
            }
        }
    }
}
