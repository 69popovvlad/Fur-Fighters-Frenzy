namespace Core.Collections.Utilities
{
    public abstract class Task
    {
        protected internal  int Priority;
        protected internal  bool Started;
        protected internal  bool Rejected;

        protected internal abstract void OnStarted();
        protected internal abstract bool IsFinished(float delta);
        protected internal abstract void OnFinished();
    }
}