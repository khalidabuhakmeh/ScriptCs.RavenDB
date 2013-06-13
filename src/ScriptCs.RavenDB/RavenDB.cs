using System;
using System.Collections.Generic;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
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
                RunInMemory = runInMemory
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
                Url = url
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
    }
}
