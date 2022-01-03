using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KeepAlive
{
    public class Runner : IDisposable
    {
        private bool disposedValue;

        TimeSpan PollingDelay { get; set; }
        Process Process { get; set; } = null;

        ProcessStartInfo StartInfo { get; set; }
        bool HasStarted { get; set; }
        public string Name() => Path.GetFileNameWithoutExtension(this.StartInfo.FileName);

        public bool IsProcessRunning(CancellationToken cancel)
        {
            if (this.Process?.HasExited??true)
            {
                Process[] processes = Process.GetProcessesByName(this.Name());
                return !processes.IsEmpty();
            }

            return true;
        }

        public async Task RunProcess(CancellationToken cancel)
        {
            while (!cancel.IsCancellationRequested) 
            {
                if (!this.IsProcessRunning(cancel))
                {
                    if (this.Process != null)
                    {
                        this.Close();
                    }

                    this.Process = new Process();
                    this.Process.StartInfo = this.StartInfo;
                    this.HasStarted = this.Process.Start();
                }
                await Task.Delay(this.PollingDelay, cancel);
            }

            this.Dispose();

        }

        public void Close()
        {
            if (this.Process?.HasExited??true)
            {

            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue=true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Runner()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
