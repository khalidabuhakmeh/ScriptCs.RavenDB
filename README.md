ScriptCs.RavenDB
================

A Script Pack for ScriptCS that exposes RavenDB to the ScriptCS engine. To use the code, just add a reference to this project to your packages.config. 

     PM > Install-Package ScriptCs.RavenDB 

## Sample

```csharp

var ravenDb = Require<RavenDB>();
ravenDb.CreateEmbeddableDocumentStore(runInMemory: true);

public class Entry {
	public string Id { get;set; }
	public string Description { get;set; }

	public override string ToString() {
		return Description;
	}
}
var doc = new Entry { 
	Id = "scriptcs-ravendb",
	Description = "scriptcs loves RavenDB"
};

using (var session = ravenDb.OpenSession()) {
	session.Store(doc);
	session.SaveChanges();
}

using (var session = ravenDb.OpenSession()) {
	var loaded = session.Load<Entry>(doc.Id);
	Console.WriteLine(loaded);
}

Console.ReadKey()

```

## Usage

You can use this script pack in multiple ways, which are listed below. Note that you will need some knowledge of RavenDB.

- Embedded DocumentStore
- Remote DocumentStore
- The script pack will track multiple document stores for you, but most likely you just need one. The stores are referenced by name, and the first one has the name of "default".

## Gotchas

- When dealing with types, Roslyn will create HTTP Header unfriendly names that could cause issues with RavenDB. The script pack resolves those issues. If you decide to use the DocumentStore directly without initializing it through the script pack then you might experience these issues.