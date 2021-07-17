using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDJDS_SFR
{
    // Scores are saved as Unsigned 16-bit Integers, they're seperated by "00" hex blocks. (they go from the highest to the lowest)

    public class Score
    {
        public Score(UInt16 points, int index)
        {
            Points = points;
            Index = index;
        }

        public UInt16 Points { get; set; }
        public int Index { get; set; }
    }
}
