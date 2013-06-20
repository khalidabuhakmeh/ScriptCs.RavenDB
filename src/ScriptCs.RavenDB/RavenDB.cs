using System;
using System.Collections.Generic;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Database.Extensions;
using ScriptCs.Contracts;

namespace ScriptCs.RavenDB
{
    public class RavenDB : IScriptPackContext, IDisposable
    {
        public IDictionary<string, IDocumentStore> Stores { get; private set; }

        public RavenDB()
        {
            Stores = new Dictionary<string, IDocumentStore>();
        }

        public EmbeddableDocumentStore CreateEmbeddableDocumentStore(bool runInMemory = false, string store = "default")
        {
            var documentStore = new EmbeddableDocumentStore
            {
                UseEmbeddedHttpServer = true,
                RunInMemory = runInMemory,
                Conventions = { FindClrTypeName = FindClrTypeName }
            };
            documentStore.Initialize();

            var url = string.Format("http://localhost:{0}", documentStore.Configuration.Port);
            Console.WriteLine("RavenDB started, listening on {0}.", url);

            Stores.Add(store, documentStore);

            return documentStore;
        }

        public IDocumentStore CreateRemoteDocumentStore(string url, string name = "default")
        {
            var documentStore = new DocumentStore
            {
                Url = url,
                Conventions = { FindClrTypeName = FindClrTypeName }
            }.Initialize();

            Stores.Add(name, documentStore);
            return documentStore;
        }

        public IDocumentSession OpenSession(string store = "default", string database = null)
        {
            return string.IsNullOrWhiteSpace(database)
                ? Stores[store].OpenSession()
                : Stores[store].OpenSession(database);
        }

        public void Dispose()
        {
            foreach (var documentStore in Stores.Values)
                documentStore.Dispose();

            Stores.Clear();
        }

        private static string FindClrTypeName(Type type)
        {
            var name = ReflectionUtil.GetFullNameWithoutVersionInformation(type);
            // the ℛ is an artifact of the Roslyn compiler when married with reflection
            // it will blow up RavenDB if not handeled
            return name.Contains("ℛ") ? MonoHttpUtility.UrlEncode(name) : name;
        }
    }
}
