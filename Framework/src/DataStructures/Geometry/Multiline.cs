using System.Collections.Generic;

namespace FavobeanGames.Framework.DataStructures.Geometry
{
    public class Multiline : Geometry
    {
        public List<Line> Lines;

        public Multiline()
        {
        }

        public Multiline(params Line[] lines)
        {
            foreach (var line in lines) Lines.Add(line);
        }
    }
}