using System;

namespace Core.Processing.Editor
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class PostprocessorStepAttribute: Attribute
    {
        public int Priority { get; private set; }
        
        public PostprocessorStepAttribute(int priority)
        {
            Priority = priority;
        }
    }
}