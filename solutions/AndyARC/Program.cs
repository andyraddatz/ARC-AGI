using AndyARC.Core;

// test rhizome uniqueness HashSet
var rhz = new Rhizome();

var ref1 = Guid.NewGuid();
var ref2 = Guid.NewGuid();
var added = rhz.RhizomeUnits.Add(new (Guid.NewGuid(), Guid.NewGuid(), "test"));
added = rhz.RhizomeUnits.Add(new (ref1, ref2, "test"));
added = rhz.RhizomeUnits.Add(new (ref1, ref2, "test"));

_ = rhz.Ontology.Append("test2");

Console.WriteLine(rhz.RhizomeUnits.Count);

// pair uniqueness
// rhz.NamedRhizomePairs.Add("test", []);
// added = rhz.NamedRhizomePairs["test"].Add(new Tuple<Guid, Guid>(ref1, ref2));
// added = rhz.NamedRhizomePairs["test"].Add(new Tuple<Guid, Guid>(ref1, ref2));
// added = rhz.NamedRhizomePairs["test"].Add(new Tuple<Guid, Guid>(Guid.NewGuid(), ref2));

// added = rhz.NamedRhizomePairs.TryAdd("test2", []);
// added = rhz.NamedRhizomePairs["test2"].Add(new Tuple<Guid, Guid>(Guid.NewGuid(), ref2));

// Console.WriteLine(rhz.NamedRhizomePairs["test"].Count);