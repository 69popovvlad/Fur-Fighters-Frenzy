using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Application;
using Core.Tasks.Progress;
using Core.Tasks.ThreadTrampolines;
using Core.Utilities;
using UnityEngine;
using TaskExtensions = Core.Tasks.TaskExtensions;

namespace Client.Bootstrappers
{
    [DefaultExecutionOrder(-50)]
    public class Bootstrapper : MonoBehaviourSingleton<Bootstrapper>
    {
        public static ProgressObserver ProgressObserver;
        public static BootstrapperArguments Arguments;
        
        private static bool _isFinished;

        private readonly BootstrapperBase[] _bootstrappers =
        {
            new UiBootstrapper(),
        };

        [SerializeField] private int _targetFrameRate = 60;

        private int _currentBootstrapper = -1;
        private IDisposable _subscribes;

        private void Awake()
        {
            if (_isFinished)
            {
                DestroyImmediate(gameObject);
                return;
            }

            Application.targetFrameRate = _targetFrameRate;
            
            UnityTask.Initialize(SynchronizationContext.Current, TaskScheduler.Default);
            ProgressObserver = new ProgressObserver(ApplicationLifeToken.LinkedCancellationSource);
            
            ParseArguments();
            DisableLogs();
            
            Debug.Log($"{nameof(Bootstrapper)} was started");

            OnEnd();
        }

        private void ParseArguments()
        {
            Arguments = new BootstrapperArguments(Environment.GetCommandLineArgs());
        }

        private void DisableLogs()
        {
#if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
#elif UNITY_IPHONE || UNITY_IOS
            Debug.unityLogger.logEnabled = false;
#elif UNITY_ANDROID
            Debug.unityLogger.logEnabled = false;
#endif
        }

        private void Update()
        {
            if (_currentBootstrapper < _bootstrappers.Length)
            {
                _bootstrappers[_currentBootstrapper].Update(Time.deltaTime);
            }
        }

        private void OnEnd()
        {
            _subscribes?.Dispose();

            if (++_currentBootstrapper < _bootstrappers.Length)
            {
                var bootstrapper = _bootstrappers[_currentBootstrapper];
                bootstrapper.OnEnd += OnEnd;
                _subscribes = new Disposable(_ => bootstrapper.OnEnd -= OnEnd);

                ProgressObserver.AddTask(new BootstrapperJob(bootstrapper.Name, TaskExtensions.WaitUntil(bootstrapper.IsFinished)));
                bootstrapper.Run();
            }
            else
            {
                ApplicationContext.Instance.Run();
            }

            ProgressObserver.Run();
            _isFinished = true;
        }
    }
}
