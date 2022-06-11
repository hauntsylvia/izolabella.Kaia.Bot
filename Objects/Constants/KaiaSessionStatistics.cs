using System.Reflection;

namespace Kaia.Bot.Objects.Constants
{
    internal static class KaiaSessionStatistics
    {
        internal static int MessageReceiverFailureCount { get; set; }
        internal static Version? Version => Assembly.GetAssembly(typeof(KaiaSessionStatistics))?.GetName().Version;
    }
}
