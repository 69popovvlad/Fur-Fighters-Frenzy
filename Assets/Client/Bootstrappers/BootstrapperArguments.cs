using System.Collections.Generic;

namespace Client.Bootstrappers
{
    public readonly struct BootstrapperArguments
    {
        public readonly bool IsServer;
        public readonly string ServerIp;
        public readonly ushort ServerPort;

        public BootstrapperArguments(IReadOnlyList<string> args) : this()
        {
            for (int i = 0, length = args.Count; i < length; ++i)
            {
                var argument = args[i];
                switch (argument)
                {
                    case "--server":
                        IsServer = true;
                        continue;
                    
                    case "-ip":
                        ServerIp = args[++i];
                        continue;
                    
                    case "-port":
                        ServerPort = ushort.Parse(args[++i]);
                        continue;
                }
            }
        }
    }
}