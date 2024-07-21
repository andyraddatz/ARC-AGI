using AndyARC.Core;

// test rhizome uniqueness HashSet
var rhz = new Rhizome();

var ref1 = Guid.NewGuid();
var ref2 = Guid.NewGuid();
var added = rhz.RhizomeUnits.Add(new(Guid.NewGuid(), Guid.NewGuid(), "test"));
added = rhz.RhizomeUnits.Add(new(ref1, ref2, "test"));
added = rhz.RhizomeUnits.Add(new(ref1, ref2, "test"));

_ = rhz.Ontology.Append("test2");

Console.WriteLine(rhz.RhizomeUnits.Count);

// todo: define an ARC-AGI solver with a rhizome structure
// todo: open 746b3537.json
ARCSample testSample = new([[1, 1, 2, 3, 3, 3, 8, 8, 4], [1, 1, 2, 3, 3, 3, 8, 8, 4], [1, 1, 2, 3, 3, 3, 8, 8, 4], [1, 1, 2, 3, 3, 3, 8, 8, 4]], [[1, 2, 3, 8, 4]]);
var pz = new Puzzle(
    train: [new([[1, 1, 1], [2, 2, 2], [1, 1, 1]], [[1], [2], [1]]), new([[3, 4, 6], [3, 4, 6], [3, 4, 6]], [[3, 4, 6]]), new([[2, 3, 3, 8, 1], [2, 3, 3, 8, 1], [2, 3, 3, 8, 1]], [[2, 3, 8, 1]]), new([[2, 2], [6, 6], [8, 8], [8, 8]], [[2], [6], [8]]), new([[4, 4, 4, 4], [4, 4, 4, 4], [2, 2, 2, 2], [2, 2, 2, 2], [8, 8, 8, 8], [3, 3, 3, 3]], [[4], [2], [8], [3]])],
    test: [testSample]
);

var solved = pz.TrySolveTest([testSample]);

Console.WriteLine(solved);

