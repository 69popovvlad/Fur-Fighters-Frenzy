using System;
using System.Collections.Generic;
using System.Reflection;
using Core.Reflection;
using JetBrains.Annotations;
using UnityEditor;

namespace Core.Processing.Editor
{
    public sealed class ProjectProcessing: AssetPostprocessor
    {
        private static readonly List<PostProcessorStep> _steps = new List<PostProcessorStep>();

        private delegate void PostProcessorStep(string[] imported, string[] deleted, string[] moved,
            string[] movedFrom);

        static ProjectProcessing()
        {
            var bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            foreach (var (type, methodInfo, _) in ReflectionUtilities
                         .MethodsWithAttributeType<PostprocessorStepAttribute>(AssembliesType.My,
                             null, true, bindingFlags))
            {
                var rawDelegate = Delegate.CreateDelegate(typeof(PostProcessorStep), methodInfo);

                if (rawDelegate is PostProcessorStep stepDelegate)
                {
                    _steps.Add(stepDelegate);
                }
                else
                {
                    throw new CustomAttributeFormatException(
                        $"Method in {type} with attribute {nameof(PostprocessorStepAttribute)} has semantics different from {nameof(PostProcessorStep)} delegate");
                }
            }
        }


        [UsedImplicitly]
        private static void OnPostprocessAllAssets(string[] imported, string[] deleted, string[] moved,
            string[] movedFrom)
        {
            for (int i = 0, length = _steps.Count; i < length; ++i)
            {
                _steps[i].Invoke(imported, deleted, moved, movedFrom);
            }
        }
    }
}