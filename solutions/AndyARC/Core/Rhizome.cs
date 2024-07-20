using System.Collections.Immutable;

namespace AndyARC.Core;
public record RhizomeUnit(Guid Ref1, Guid Ref2, string OntologyRef);
public class Rhizome
{
    
    public HashSet<RhizomeUnit> RhizomeUnits { get; set; } = [];
    public IReadOnlySet<string> Ontology => RhizomeUnits.Select(x => x.OntologyRef).ToImmutableHashSet();
    // public IReadOnlySet<string> Ontology2 => NamedRhizomePairs.Keys.ToImmutableHashSet();
    // is this just as good? no, I think we actually want to allow multiple entries for same key
    // public Dictionary<string, HashSet<Tuple<Guid,Guid>>> NamedRhizomePairs { get; set; } = [];
}