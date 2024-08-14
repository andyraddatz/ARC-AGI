using System.Collections.Immutable;

// Heavy inspiration from https://fabian-kostadinov.github.io/2014/09/09/implementation-of-rhizomes/
// should consider this if performance becomes a problem http://fabian-kostadinov.github.io/2015/08/18/implementation-of-rhizomes-2/
namespace AndyARC.Core;
public record RhizomeUnit(Guid Ref1, Guid Ref2, string OntologyRef);

public class Rhizome
{
    
    public HashSet<RhizomeUnit> RhizomeUnits { get; set; } = [];
    public IReadOnlySet<string> Ontology => RhizomeUnits.Select(x => x.OntologyRef).ToImmutableHashSet();

    public static Rhizome SampleRhizome()
    {
        Guid _selfId = Guid.NewGuid();
        Guid _worldId = Guid.NewGuid();
        Guid _selfWorldId = Guid.NewGuid();
        Guid _gridId = Guid.NewGuid();
        var rhizome = new Rhizome {
            RhizomeUnits = [
                new(_selfId, _selfId, "self"),
                new(_worldId, _worldId, "world"),
                new(_selfId, _worldId, "self-world"),
                new(_worldId, _selfId, "world-self"),
                new(_selfWorldId, _selfWorldId, "self-world"),

                // new(_gridId, _gridId, "grid"),
            ]
        };
        return rhizome;
    }
}