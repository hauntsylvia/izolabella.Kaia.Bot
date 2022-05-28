using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Constants
{
    internal static class KaiaSessionStatistics
    {
        internal static int MessageReceiverFailureCount { get; set; }
        internal static Version? Version => Assembly.GetAssembly(typeof(KaiaSessionStatistics))?.GetName().Version;
    }
}
