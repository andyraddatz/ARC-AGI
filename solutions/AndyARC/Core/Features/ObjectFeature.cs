namespace AndyARC.Core.Features;
/// <summary>
/// <para>Object cohesion: Ability to parse grids into “objects” based on continuity criteria including
/// color continuity or spatial contiguity ability to parse grids into zones, partitions.
/// </para>
/// <para>
/// Object persistence: Objects are assumed to persist despite the presence of noise or occlusion by other objects. 
/// In many cases (but not all) objects from the input persist
/// on the output grid, often in a transformed form. Common geometric transformations of
/// objects are covered in category 4, “basic geometry and topology priors”.
/// </para>
/// <para>
/// Object influence via contact: Many tasks feature physical contact between objects (e.g.
/// one object being translated until it is in contact with another or a line “growing”
/// until it “rebounds” against another object
/// </para>
/// </summary>
public class ObjectFeature
{
    public required Point StartXY { get; set; }
    public required Point EndXY { get; set; }
    public required int[][] RawData { get; set; }
    public int Height => EndXY.Y - StartXY.Y;
    public int Width => EndXY.X - StartXY.X;
    public bool IsSquare => Height == Width;
    public IEnumerable<Point> Symbol { get; set; } = [];
    public IEnumerable<Point> Noise { get; set; } = [];
    public IEnumerable<ObjectFeature> OccludedBy { get; set; } = [];
    public IEnumerable<ObjectFeature> Contains { get; set; } = [];
    public IEnumerable<ObjectFeature> Touches { get; set; } = [];
}
