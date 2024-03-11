
using System.Threading.Tasks;
using Core.Tasks.Progress;

namespace Client.Bootstrappers
{
    public class BootstrapperJob: JobObserver<BootstrapperJob>
    {
        public override string Description => _description;

        private readonly string _description;

        public BootstrapperJob(string description, Task task) : base(task)
        {
            _description = description;
        }
    }
}