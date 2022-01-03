using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepAlive
{
    /// <summary>
    /// Target application or document to keep alive.
    /// </summary>
    /// <remarks>XML Documentation shamelessly copied from ProcessStartInfo metadata.</remarks>
    public class TargetInfo
    {
        /// <summary>
        /// Initializes a new instance of the KeepAlive.TargetInfo class without
        /// specifying a file name with which to start the process.
        /// </summary>
        public TargetInfo()
        {
            // TBD
        }

        /// <summary>
        ///     Initializes a new instance of the System.Diagnostics.ProcessStartInfo class and
        ///     specifies a file name such as an application or document with which to start
        ///     the process.
        /// </summary>
        /// <param name="filePath">An application or document with which to start a process.</param>
        public TargetInfo(string filePath)
            : this()
        {
            this.FilePath = filePath;
        }

        ///.<summary>
        ///.   Initializes a new instance of the System.Diagnostics.ProcessStartInfo class,
        ///.   specifies an application file name with which to start the process, and specifies
        ///.   a set of command-line arguments to pass to the application.
        ///.</summary>
        /// <param name="fileName">An application or document with which to start a process.</param>
        ///.<param name="arguments">Command-line arguments to pass to the application when the process starts.</param>
        public TargetInfo(string fileName, string arguments)
            : this(fileName)
        {
            this.Arguments = arguments;
        }

        /// <summary>
        ///    Gets or sets the application or document to start.
        /// </summary>
        /// <returns>
        ///    The name of the application to start, or the name of a document of a file type
        ///    that is associated with an application and that has a default open action available
        ///    to it. The default is an empty string ("").
        /// </returns>
        [Editor("System.Diagnostics.Design.StartFileNameEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
        public string FilePath { get; set; }

        /// <summary>
        /// Gets application's file name.
        /// </summary>
        /// <returns></returns>
        public string FileName()
            => Utility.IsEmpty(this.FilePath)
            ? null
            : Path.GetFileName(this.FilePath);

        /// <summary>
        ///    Gets or sets the set of command-line arguments to use when starting the application.
        /// </summary>
        /// <returns>
        ///    A single string containing the arguments to pass to the target application specified
        ///    in the System.Diagnostics.ProcessStartInfo.FileName property. The default is
        ///    an empty string ("").
        /// </returns>
        public string Arguments { get; set; }

        /// <summary>
        ///    When the System.Diagnostics.ProcessStartInfo.UseShellExecute property is false,
        ///    gets or sets the working directory for the process to be started. When System.Diagnostics.ProcessStartInfo.UseShellExecute
        ///    is true, gets or sets the directory that contains the process to be started.
        /// </summary>
        /// <returns>
        ///    When System.Diagnostics.ProcessStartInfo.UseShellExecute is true, the fully qualified
        ///    name of the directory that contains the process to be started. When the System.Diagnostics.ProcessStartInfo.UseShellExecute
        ///    property is false, the working directory for the process to be started. The default
        ///    is an empty string ("").
        /// </returns>
        [Editor("System.Diagnostics.Design.WorkingDirectoryEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
        public string WorkingDirectory { get; set; }
    }
}
