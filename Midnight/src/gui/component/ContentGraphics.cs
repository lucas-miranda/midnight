using System.Collections.Generic;

namespace Midnight.GUI;

public class ContentGraphics : Component {
    public List<GraphicDisplayer> Entries { get; } = new();
}
