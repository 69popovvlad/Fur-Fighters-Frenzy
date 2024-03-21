using Client.Bootstrappers;
using FishNet;
using FishNet.Transporting;
using FishNet.Transporting.Tugboat;
using UnityEngine;

namespace Client.Network
{
    [DefaultExecutionOrder(short.MinValue)]
    public class ServerStartControl : MonoBehaviour
    {
        [SerializeField] private Tugboat _tugboat;

        private LocalConnectionState _serverState = LocalConnectionState.Stopped;

        private void Awake()
        {
            if (!Bootstrapper.Arguments.IsServer)
            {
                Destroy(this);
                return;
            }

            InstanceFinder.ServerManager.OnServerConnectionState += ServerManager_OnServerConnectionState;

            _tugboat.SetServerBindAddress(Bootstrapper.Arguments.ServerIp, IPAddressType.IPv4);
            _tugboat.SetPort(Bootstrapper.Arguments.ServerPort);
            _tugboat.SetMaximumClients(Bootstrapper.Arguments.MaxPlayers);
            
            Debug.Log($"Start with params: -ip {Bootstrapper.Arguments.ServerIp} -p {Bootstrapper.Arguments.ServerPort}");

            StartServer();
        }

        private void OnDestroy()
        {
            InstanceFinder.ServerManager.OnServerConnectionState -= ServerManager_OnServerConnectionState;
        }

        private void StartServer()
        {
            if (_serverState != LocalConnectionState.Stopped)
            {
                Debug.Log("Stopping server hosting");
                InstanceFinder.ServerManager.StopConnection(true);
            }
            else
            {
                Debug.Log("Starting server hosting");
                InstanceFinder.ServerManager.StartConnection();
            }
        }

        private void ServerManager_OnServerConnectionState(ServerConnectionStateArgs obj)
        {
            _serverState = obj.ConnectionState;
        }
    }
}