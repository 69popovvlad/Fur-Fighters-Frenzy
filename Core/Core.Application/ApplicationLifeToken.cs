using System.Threading;
using Core.Application.Events;
using UnityEngine;

namespace Core.Application
{
    public class ApplicationLifeToken: IApplicationResource, IUnityApplicationQuitListener
    {
        private static readonly CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();
        public static CancellationTokenSource LinkedCancellationSource => CancellationTokenSource.CreateLinkedTokenSource(Token);
        
        public static CancellationToken Token => CancellationTokenSource?.Token ?? CancellationToken.None;

        public void OnApplicationQuit()
        {
            CancellationTokenSource?.Cancel();
            Debug.Log("<color=cyan>All application life time tasks were canceled</color>");
        }
    }
}