using System;

namespace Core.Processing.Editor
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class PostprocessorAttribute : Attribute
    {
        public readonly int Priority;

        public PostprocessorAttribute(int priority)
        {
            Priority = priority;
        }
    }
}