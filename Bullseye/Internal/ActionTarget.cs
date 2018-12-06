namespace Bullseye.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class ActionTarget : Target
    {
        private readonly Func<Task> action;

        public ActionTarget(TargetName name, List<TargetName> dependencies, Func<Task> action)
            : base(name, dependencies) => this.action = action;

        public override async Task RunAsync(bool dryRun, bool parallel, Logger log)
        {
            await log.Starting(this.Name).ConfigureAwait(false);

            var stopWatch = Stopwatch.StartNew();

            try
            {
                if (!dryRun)
                {
                    await this.action().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                await log.Failed(this.Name, ex, stopWatch.Elapsed.TotalMilliseconds).ConfigureAwait(false);
                throw;
            }

            await log.Succeeded(this.Name, stopWatch.Elapsed.TotalMilliseconds).ConfigureAwait(false);
        }
    }
}
