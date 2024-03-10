using System.Collections.Generic;
using Core.Collections.Utilities.Queue;

namespace Core.Collections.Utilities
{
    public abstract class TaskManager
    {
        private readonly Dictionary<long, SimplePriorityQueue<Task>> _tasksQueue = new Dictionary<long, SimplePriorityQueue<Task>>();

        protected abstract bool Validate();
        
        public void AddTask(Task task, long objectHash)
        {
            if (!_tasksQueue.TryGetValue(objectHash, out var queue))
            {
                _tasksQueue[objectHash] = queue = new SimplePriorityQueue<Task>();
            }

            queue.Enqueue(task, task.Priority);
        }

        public void TaskUpdate(float delta)
        {
            foreach (var (_, queue) in new Dictionary<long, SimplePriorityQueue<Task>>(_tasksQueue))
            {
                if (!Validate())
                {
                    return;
                }
                
                if (queue.Count <= 0)
                {
                    continue;
                }
                
                var task = queue.Peek();
                if (!task.Started)
                {
                    task.OnStarted();
                    task.Started = true;
                }

                if (!task.Rejected && !task.IsFinished(delta))
                {
                    continue;
                }
                
                queue.Dequeue();
                task.OnFinished();
            }
        }

        public void ReleaseTasksFor(long objectHash) =>
            _tasksQueue.Remove(objectHash);
    }
}