using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDJDS_SFR
{
    // Scores are saved as Signed 32-bit Integers (they go from the highest to the lowest)

    public class Score
    {
        public Score(int points, int index)
        {
            Points = points;
            Index = index;
        }

        public int Points { get; set; }
        public int Index { get; set; }
    }
}
