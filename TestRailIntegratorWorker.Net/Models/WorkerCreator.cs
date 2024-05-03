using TestRailIntegratorWorker.Interfaces;

namespace TestRailIntegratorWorker.Models
{
    abstract class WorkerCreator
    {
        public abstract IWorker CreateWorker();

        public void Run()
        {
            IWorker worker = CreateWorker();

            worker.Run();
        }
    }
}
