using ScriptCs.Contracts;

namespace ScriptCs.RavenDB
{
    public class ScriptPack : IScriptPack
    {
        private static readonly RavenDB RavenDb;

        static ScriptPack()
        {
            RavenDb = new RavenDB();
        }

        public void Initialize(IScriptPackSession session)
        {
            var namespaces = new[]
            {
                "Raven.Client",
                "Raven.Client.Embedded",
                "Raven.Client.Document",
                "Raven.Client.Indexes",
                "Raven.Client.Extensions",
                "Raven.Client.Linq",
                "Raven.Client.Bundles.MoreLikeThis",
                "Raven.Client.Bundles.Versioning"
            };

            foreach (var ns in namespaces)
                session.ImportNamespace(ns);
        }

        public IScriptPackContext GetContext()
        {
            return RavenDb;
        }

        public void Terminate()
        {
            RavenDb.Dispose();
        }
    }
}
