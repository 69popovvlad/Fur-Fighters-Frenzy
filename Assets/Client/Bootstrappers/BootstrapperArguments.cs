using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace Client.Bootstrappers
{
    using static BootstrapperArgumentsKeys;
    
    public readonly struct BootstrapperArguments
    {
        public readonly bool IsServer;
        public readonly string ServerIp;
        public readonly ushort ServerPort;
        public readonly string ServerPassword;
        public readonly int MaxPlayers;

        public BootstrapperArguments(IReadOnlyList<string> args) : this()
        {
            for (int i = 0, iLen = args.Count; i < iLen; ++i)
            {
                switch (args[i])
                {
                    case IsServerKey:
                        IsServer = IsServerParse(string.Empty);
                        break;

                    case ServerBindAddressIpv4Key when i + 1 < iLen:
                        ServerIp = IpParse(args[++i]);
                        break;

                    case ServerPortKey when i + 1 < iLen:
                        ServerPort = PortParse(args[++i]);
                        break;

                    case ServerPassKey when i + 1 < iLen:
                        ServerPassword = PasswordParse(args[++i]);
                        ++i;
                        break;
                    
                    case MaxPlayersKey when i + 1 < iLen:
                        MaxPlayers = MaxPlayersParse(args[++i]);
                        break;
                }
            }
        }

        private static bool IsServerParse(string arg)
        {
            return true;
        }

        private static string IpParse(string ipAddressString)
        {
            try
            {
                if (!IPAddress.TryParse(ipAddressString, out var ipAddress))
                {
                    throw new Exception($"{ipAddressString} is invalid ip address");
                }

                return ipAddressString;
            }
            catch (Exception error)
            {
                Debug.LogError(error);
                return "localhost";
            }
        }

        private static ushort PortParse(string arg)
        {
            try
            {
                return ushort.Parse(arg);
            }
            catch (Exception error)
            {
                Debug.LogError(error);
                return 0;
            }
        }

        private static string PasswordParse(string pass)
        {
            return pass;
        }
        
        private static int MaxPlayersParse(string arg)
        {
            return Mathf.Min(2, int.Parse(arg));
        }
    }
}