using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KeepAlive
{
    public class Worker : BackgroundService
    {
        private ILogger<Worker> _logger { get; set; }

        public TargetInfo _target { get; set; }

        public Process _process { get; set; }

        public Worker(ILogger<Worker> logger)
        {
            this._logger = logger;
            this._target = new TargetInfo("notepad.exe");
            this._process = null;
        }

        protected override async Task ExecuteAsync(CancellationToken cancel)
        {
            // Loop until canceled.
            while (!cancel.IsCancellationRequested)
            {
                // Wait here.
                await Task.Delay(1000, cancel);

                // Check if already running.
                if (this.HasTarget(cancel))
                {
                    continue;
                }

                if (RunTarget(cancel))
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Has target started.
        /// </summary>
        /// <param name="cancel">Cancellation token for canceling the task.</param>
        /// <returns>True indicating cancellation requested or target already running.</returns>
        private bool HasTarget(CancellationToken cancel)
        {
            if (cancel.IsCancellationRequested)
            {
                return true;
            }

            // Note: Code utility method to allow more than one (1) process named this._target.FileName().
            if (Utility.IsRunning(this._target.FilePath))
            {
                this._logger.LogInformation($"{DateTimeOffset.Now}: {this._target.FileName()} running.");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Run target.
        /// </summary>
        /// <param name="cancel">Cancellation token for canceling the task.</param>
        /// <returns>True indicating cancellation requested target aborted.</returns>
        private bool RunTarget(CancellationToken cancel)
        {
            if (cancel.IsCancellationRequested)
            {
                return true;
            }

            ProcessStartInfo info = new ProcessStartInfo(this._target.FilePath, this._target.Arguments);
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;

            try
            {
                int rc = Utility.Disposer(() => new Process(), (Process p) =>
                {
                    p.StartInfo = info;
                    p.OutputDataReceived += new DataReceivedEventHandler((s, e) =>
                    {
                        this._logger.LogInformation(e.Data);
                        if (cancel.IsCancellationRequested)
                        {
                            return;
                        }

                    });
                    p.BeginOutputReadLine();
                    p.ErrorDataReceived += new DataReceivedEventHandler((s, e) =>
                    {
                        this._logger.LogWarning(e.Data);
                        if (cancel.IsCancellationRequested)
                        {
                            return;
                        }
                    });
                    p.BeginErrorReadLine();

                    p.Start();
                    p.WaitForExit();

                    return p.ExitCode;
                });

                this._logger.LogInformation($"{this._target.FileName()} exited with return code {rc}.");

            }
            catch (Exception ex)
            {
                this._logger.LogError($"{ex.GetType().Name}: {ex.Message}");

                return true;
            }

            return false;
        }
    }
}