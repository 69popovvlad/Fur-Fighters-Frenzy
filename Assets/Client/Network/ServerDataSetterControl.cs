using FishNet.Transporting;
using FishNet.Transporting.Tugboat;
using TMPro;
using UnityEngine;

namespace Client.Network
{
    public class ServerDataSetterControl : MonoBehaviour
    {
        [SerializeField] private Tugboat _tugboat;
        [SerializeField] private TMP_InputField _ipField;
        [SerializeField] private TMP_InputField _portField;
        [SerializeField] private TextMeshProUGUI _info;

        private void Awake()
        {
            _ipField.onValueChanged.AddListener(OnIpChanged);
            _portField.onValueChanged.AddListener(OnPortChanged);
        }

        private void OnDestroy()
        {
            _ipField.onValueChanged.RemoveListener(OnIpChanged);
            _portField.onValueChanged.RemoveListener(OnPortChanged);
        }

        private void OnIpChanged(string ip)
        {
            _tugboat.SetServerBindAddress(ip, IPAddressType.IPv4);
        }

        private void OnPortChanged(string port)
        {
            try
            {
                _tugboat.SetPort(ushort.Parse(port));
            }
            catch
            {
                _info.text = "Port is invalid";
            }
        }
    }
}