using EnsureThat;

namespace MyCouch
{
    internal static class Ensures
    {
        internal static void EnsureValid(this CopyDocumentCommand cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();
            Ensure.That(cmd.SrcId, "cmd.SrcId").IsNotNullOrWhiteSpace();
            Ensure.That(cmd.NewId, "cmd.NewId").IsNotNullOrWhiteSpace();
        }

        internal static void EnsureValid(this IViewQuery query)
        {
            Ensure.That(query, "query").IsNotNull();
        }
    }
}
