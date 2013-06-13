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