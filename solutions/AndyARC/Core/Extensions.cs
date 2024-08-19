using AndyARC.Core.Features;

namespace AndyARC.Core;

public static class Extensions
{
    // c. Numbers and Counting priors:

    // Many ARC tasks involve counting or sorting objects (e.g. sorting by size), comparing
    // numbers (e.g. which shape or symbol appears the most (e.g. figure 10)? The least? The
    // same number of times? Which is the largest object? The smallest? Which objects are the
    // same size?), or repeating a pattern for a fixed number of time. The notions of addition and
    // subtraction are also featured (as they are part of the Core Knowledge number system as per
    // [85]). All quantities featured in ARC are smaller than approximately 10.
    public static IOrderedEnumerable<ObjectBox>? SortBySize(this IEnumerable<ObjectBox> objects)
    {
        return objects.OrderBy(o => o);
    }
    
}