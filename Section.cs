using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDJDS_SFR
{
    // A valid DDJDS save file should have 8 sections
    // They're all 64 bytes long (offsets should therefore be as following: 0 (0x00), 64 (0x40), 128 (0x80), 192 (0xC0) etc...);

    // Expected Layout:
    // |- Sections 1-6: Score Data
    // |- Sections   7: "DOODLEJUMPSAVES"
    // |- Sections   8: Signatures

    public class Section
    {
        public Section(byte[] sectionData, int sectionIndex)
        {
            rawData = sectionData;
            SectionNumber = sectionIndex;
            InitializeSection();
        }

        private void InitializeSection()
        {
            int position = 0;
            List<Score> tempScores = new List<Score>();
            for (int x = 0; x < 5; x++)
            {
                tempScores.Add(new Score(BitConverter.ToInt32(rawData, position), x));
                position += 4;
            }
            scoreMarkers = BitConverter.ToUInt32(rawData, 40) != 1;
            sound = BitConverter.ToUInt32(rawData, 44) != 1;
            Scores = tempScores.ToArray();
        }

        public int SectionNumber { get; set; }
        public byte[] rawData { get; set; }
        public Score[] Scores { get; set; }
        public bool scoreMarkers { get; set; }
        public bool sound { get; set; }
    }
}
